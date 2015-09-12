// RTM.Tools
// RTM.Component.CameraMovementDetector
// VectorsCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV.Util;
using RTM.Calculator.CameraCalibration;
using RTM.Component.CameraMovementDetector.Configuration;
using RTM.Component.CameraMovementDetector.Filter;
using RTM.DTO;

namespace RTM.Component.CameraMovementDetector.VectorsCalculator
{
    public class VectorsCalculator : IVectorsCalculator
    {
        private readonly IVectorsFilter vectorsFilter;
        private readonly IComponentConfiguration configuration;
        private readonly ICameraCalibration cameraCalibration;

        public VectorsCalculator(IComponentConfiguration componentConfiguration, ICameraCalibration calibration,
            IVectorsFilter filter)
        {
            cameraCalibration = calibration;
            vectorsFilter = filter;
            configuration = componentConfiguration;
        }
        
        public Vectors Calculate(VectorOfPointF cornerPoints, Size imageSize)
        {
            cameraCalibration.Calibrate(new VectorOfVectorOfPointF(cornerPoints), imageSize, configuration.InnerCornersPerChessboardCols, configuration.InnerCornersPerChessboardRows);

            var vectors = vectorsFilter.Correct(cameraCalibration.Rotation, cameraCalibration.Translation);

            return vectors;
        }
    }
}