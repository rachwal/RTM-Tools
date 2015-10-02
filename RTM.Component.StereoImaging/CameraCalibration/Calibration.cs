// RTM.Tools
// RTM.Component.StereoImaging
// Calibration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using RTM.Calculator.StereoCalibration;
using RTM.Component.StereoImaging.Configuration;
using RTM.Component.StereoImaging.Configuration.Parameters;
using RTM.Detector.ChessboardCorners;

namespace RTM.Component.StereoImaging.CameraCalibration
{
    public class Calibration : ICalibration
    {
        private readonly IComponentConfiguration configuration;
        private readonly IChessboardCornersDetector detector;
        private readonly IStereoCalibration stereo;

        private readonly VectorOfVectorOfPointF cornersPointsLeft = new VectorOfVectorOfPointF();
        private readonly VectorOfVectorOfPointF cornersPointsRight = new VectorOfVectorOfPointF();

        public IInputArray LeftMapX => stereo.LeftMapX;
        public IInputArray LeftMapY => stereo.LeftMapY;
        public IInputArray RightMapX => stereo.RightMapX;
        public IInputArray RightMapY => stereo.RightMapY;

        public Calibration(IComponentConfiguration componentConfiguration, IChessboardCornersDetector cornersDetector,
            IStereoCalibration stereoCalibration)
        {
            stereo = stereoCalibration;
            detector = cornersDetector;
            configuration = componentConfiguration;
            configuration.CalibrationStatusChanged += OnCalibrationStatusChanged;
        }

        private void OnCalibrationStatusChanged(object sender, EventArgs e)
        {
            if (configuration.CalibrationStatus != CalibrationStatus.NotCalibrated)
            {
                return;
            }
            cornersPointsLeft.Clear();
            cornersPointsRight.Clear();
            configuration.CalibrationStatus = CalibrationStatus.CollectingFrames;
        }

        public void Calibrate(Image<Gray, byte> leftGrayImage, Image<Gray, byte> rightGrayImage)
        {
            if (configuration.CalibrationStatus == CalibrationStatus.CollectingFrames)
            {
                if (cornersPointsLeft.Size < configuration.NumCalibFrames)
                {
                    CollectFrame(leftGrayImage, rightGrayImage);
                    return;
                }
                configuration.CalibrationStatus = CalibrationStatus.Calibrating;
            }

            if (configuration.CalibrationStatus == CalibrationStatus.Calibrating)
            {
                stereo.Calibrate(cornersPointsLeft, cornersPointsRight, configuration.InnerCornersPerChessboardCols,
                    configuration.InnerCornersPerChessboardRows, leftGrayImage.Size);
                configuration.CalibrationStatus = CalibrationStatus.Calibrated;
            }
        }

        private void CollectFrame(Image<Gray, byte> left, Image<Gray, byte> right)
        {
            var cornersLeft = detector.Detect(left, configuration.InnerCornersPerChessboardCols,
                configuration.InnerCornersPerChessboardRows);
            var cornersRight = detector.Detect(right, configuration.InnerCornersPerChessboardCols,
                configuration.InnerCornersPerChessboardRows);

            var expectedSize = configuration.InnerCornersPerChessboardCols*configuration.InnerCornersPerChessboardRows;
            if (cornersLeft.Size != expectedSize || cornersRight.Size != expectedSize)
            {
                return;
            }

            cornersPointsLeft.Push(cornersLeft);
            cornersPointsRight.Push(cornersRight);

            configuration.CalibratedFrames = cornersPointsRight.Size;
        }
    }
}