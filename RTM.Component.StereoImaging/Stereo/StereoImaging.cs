// RTM.Tools
// RTM.Component.StereoImaging
// StereoImaging.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using OpenRTM.Core;
using RTM.Component.StereoImaging.CameraCalibration;
using RTM.Component.StereoImaging.Configuration;
using RTM.Component.StereoImaging.Disparity;
using RTM.Converter.CameraImage;

namespace RTM.Component.StereoImaging.Stereo
{
    public class StereoImaging : IStereoImaging
    {
        private readonly IComponentConfiguration configuration;
        private readonly ICameraImageConverter converter;
        private readonly IDisparitySolver disparity;
        private readonly ICalibration calibration;

        private volatile bool processing;

        private CameraImage rightCameraImage;
        private CameraImage leftCameraImage;
        private CameraImage disparityMap;

        public CameraImage RightCameraImage
        {
            get { return rightCameraImage; }
            set
            {
                rightCameraImage = value;
                NewRightCameraImage?.Invoke(this, EventArgs.Empty);
            }
        }

        public CameraImage LeftCameraImage
        {
            get { return leftCameraImage; }
            set
            {
                leftCameraImage = value;
                NewLeftCameraImage?.Invoke(this, EventArgs.Empty);
            }
        }

        public CameraImage DisparityMap
        {
            get { return disparityMap; }
            set
            {
                disparityMap = value;
                NewDisparityMap?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler NewRightCameraImage;
        public event EventHandler NewLeftCameraImage;
        public event EventHandler NewDisparityMap;

        public StereoImaging(IDisparitySolver disparitySolver, ICalibration cameraCalibration,
            ICameraImageConverter cameraImageConverter, IComponentConfiguration componentConfiguration)
        {
            calibration = cameraCalibration;
            disparity = disparitySolver;
            converter = cameraImageConverter;

            configuration = componentConfiguration;

            NewLeftCameraImage += OnNewCameraImage;
            NewRightCameraImage += OnNewCameraImage;
            NewDisparityMap += OnNewDisparityMap;
        }

        private void OnNewDisparityMap(object sender, EventArgs e)
        {
            processing = false;
        }

        public void ProcessLeftImage(CameraImage cameraImage)
        {
            LeftCameraImage = cameraImage;
        }

        public void ProcessRightImage(CameraImage cameraImage)
        {
            RightCameraImage = cameraImage;
        }

        private void OnNewCameraImage(object sender, EventArgs e)
        {
            if (processing || LeftCameraImage == null || RightCameraImage == null)
            {
                return;
            }

            processing = true;

            var leftBitmap = converter.ToBitmap(leftCameraImage.Copy());
            var rightBitmap = converter.ToBitmap(rightCameraImage.Copy());

            var leftGrayImage = new Image<Gray, byte>(leftBitmap);
            var rightGrayImage = new Image<Gray, byte>(rightBitmap);

            if (configuration.CalibrationStatus == CalibrationStatus.CollectingFrames)
            {
                calibration.Calibrate(leftGrayImage, rightGrayImage);
                processing = false;
                return;
            }

            Process(leftGrayImage, rightGrayImage);
        }

        private void Process(Image<Gray, byte> left, Image<Gray, byte> right)
        {
            if (configuration.CalibrationStatus == CalibrationStatus.Calibrated)
            {
                var leftImage = left.CopyBlank();
                CvInvoke.Remap(left, leftImage, calibration.LeftMapX, calibration.LeftMapY,
                    Inter.Linear);

                var rightImage = right.CopyBlank();
                CvInvoke.Remap(right, rightImage, calibration.RightMapX, calibration.RightMapY,
                    Inter.Linear);
            }

            var disparityImage = disparity.Solve(left, right);

            UpdateResult(disparityImage);
        }

        private void UpdateResult(IImage disparityImage)
        {
            var result = converter.Convert(disparityImage.Bitmap);
            result.Format = "Gray8";
            DisparityMap = result;
        }
    }
}