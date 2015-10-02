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
using RTM.Component.StereoImaging.Configuration.Parameters;
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

        public event EventHandler<CameraImage> NewDisparityMap;

        public StereoImaging(IDisparitySolver disparitySolver, ICalibration cameraCalibration,
            ICameraImageConverter cameraImageConverter, IComponentConfiguration componentConfiguration)
        {
            calibration = cameraCalibration;
            disparity = disparitySolver;
            converter = cameraImageConverter;

            configuration = componentConfiguration;
            NewDisparityMap += OnNewDisparityMap;
        }

        private void OnNewDisparityMap(object sender, CameraImage cameraImage)
        {
            processing = false;
        }

        public void ProcessLeftImage(CameraImage cameraImage)
        {
            leftCameraImage = cameraImage;
            Process();
        }

        public void ProcessRightImage(CameraImage cameraImage)
        {
            rightCameraImage = cameraImage;
            Process();
        }

        private void Process()
        {
            if (processing || leftCameraImage == null || rightCameraImage == null)
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

            var disparityMap = disparity.Solve(left, right);

            UpdateResult(disparityMap);
        }

        private void UpdateResult(IImage image)
        {
            var result = converter.Convert(image.Bitmap);
            result.Format = "Gray8";
            NewDisparityMap?.Invoke(this, result);
        }
    }
}