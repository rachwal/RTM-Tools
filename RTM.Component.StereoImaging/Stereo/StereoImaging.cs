// RTM.Tools
// RTM.Component.StereoImaging
// StereoImaging.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OpenRTM.Core;
using RTM.Calculator.StereoCalibration;
using RTM.Component.StereoImaging.Configuration;
using RTM.Component.StereoImaging.Disparity;
using RTM.Converter.CameraImage;
using RTM.Detector.ChessboardCorners;

namespace RTM.Component.StereoImaging.Stereo
{
    public class StereoImaging : IStereoImaging
    {
        private readonly IComponentConfiguration configuration;
        private readonly ICameraImageConverter converter;
        private readonly IChessboardCornersDetector detector;
        private readonly IStereoCalibration stereo;
        private readonly IDisparitySolver disparity;

        private CameraImage camera1Image;
        private CameraImage camera2Image;
        private CameraImage disparityMap;

        private CameraImage leftCameraImage;
        private CameraImage rightCameraImage;

        private readonly VectorOfVectorOfPointF cornersPointsLeft = new VectorOfVectorOfPointF();
        private readonly VectorOfVectorOfPointF cornersPointsRight = new VectorOfVectorOfPointF();

        private volatile bool processing;

        public CameraImage Camera1Image
        {
            get { return camera1Image; }
            set
            {
                camera1Image = value;
                NewCamera1Image?.Invoke(this, EventArgs.Empty);
            }
        }

        public CameraImage Camera2Image
        {
            get { return camera2Image; }
            set
            {
                camera2Image = value;
                NewCamera2Image?.Invoke(this, EventArgs.Empty);
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

        public event EventHandler NewCamera1Image;
        public event EventHandler NewCamera2Image;
        public event EventHandler NewDisparityMap;

        public StereoImaging(IChessboardCornersDetector chessboardCornersDetector, IStereoCalibration stereoStereo, IDisparitySolver disparitySolver,
            ICameraImageConverter cameraImageConverter, IComponentConfiguration componentConfiguration)
        {
            disparity = disparitySolver;
            stereo = stereoStereo;
            detector = chessboardCornersDetector;
            converter = cameraImageConverter;
            configuration = componentConfiguration;
            configuration.CalibratedChanged += OnCalibratedChanged;
        }

        private void OnCalibratedChanged(object sender, EventArgs e)
        {
            if (configuration.Calibrated)
            {
                return;
            }
            cornersPointsLeft.Clear();
            cornersPointsRight.Clear();
        }

        public void ProcessImage2(CameraImage cameraImage)
        {
            if (leftCameraImage == null)
            {
                leftCameraImage = cameraImage.Copy();
                return;
            }

            Camera2Image = cameraImage;

            if (!processing && rightCameraImage != null)
            {
                Process();
            }
        }

        public void ProcessImage1(CameraImage cameraImage)
        {
            if (rightCameraImage == null)
            {
                rightCameraImage = cameraImage.Copy();
                return;
            }

            Camera1Image = cameraImage;

            if (!processing && leftCameraImage != null)
            {
                Process();
            }
        }

        private void Process()
        {
            processing = true;

            var bitmap1 = converter.ToBitmap(leftCameraImage);
            var bitmap2 = converter.ToBitmap(rightCameraImage);

            var grayImage1 = new Image<Gray, byte>(bitmap1);
            var grayImage2 = new Image<Gray, byte>(bitmap2);

            if (cornersPointsLeft.Size < configuration.NumCalibFrames)
            {
                CollectFrame(grayImage1, grayImage2);
                return;
            }

            if (!configuration.Calibrated)
            {
                stereo.Calibrate(cornersPointsLeft, cornersPointsRight, configuration.InnerCornersPerChessboardCols,
                    configuration.InnerCornersPerChessboardRows, grayImage1.Size);
                configuration.Calibrated = true;
            }

            Image<Gray, short> disparityImage;
            MCvPoint3D32f[] points;
            disparity.Solve(grayImage1, grayImage2, out disparityImage, out points);

            UpdateResult(disparityImage);
        }

        private void UpdateResult(Image<Gray, short> disparityImage)
        {
            var result = converter.Convert(disparityImage.Bitmap);
            result.Format = "Gray8";
            DisparityMap = result;

            ResetProcessing();
        }

        private void CollectFrame(Image<Gray, byte> leftImage, Image<Gray, byte> rightImage)
        {
            var cornersLeft = detector.Detect(leftImage, configuration.InnerCornersPerChessboardCols, configuration.InnerCornersPerChessboardRows);
            var cornersRight = detector.Detect(rightImage, configuration.InnerCornersPerChessboardCols, configuration.InnerCornersPerChessboardRows);

            var expectedSize = configuration.InnerCornersPerChessboardCols * configuration.InnerCornersPerChessboardRows;
            if (cornersLeft.Size != expectedSize || cornersRight.Size != expectedSize)
            {
                ResetProcessing();
                return;
            }

            cornersPointsLeft.Push(cornersLeft);
            cornersPointsRight.Push(cornersRight);

            configuration.CalibratedFrames = cornersPointsRight.Size;

            ResetProcessing();
        }

        private void ResetProcessing()
        {
            processing = false;
            leftCameraImage = null;
            rightCameraImage = null;
        }
    }
}