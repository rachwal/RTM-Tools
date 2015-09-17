// RTM.Tools
// RTM.Calculator.StereoCalibration
// IStereoCalibration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Util;

namespace RTM.Calculator.StereoCalibration
{
    public interface IStereoCalibration
    {
        Matrix<double> Camera1Matrix { get; }
        Matrix<double> Camera1DistortionCoeffs { get; }
        Matrix<double> Camera1Rectification { get; }
        Matrix<double> Camera1Projection { get; }
        Matrix<double> Camera2Matrix { get; }
        Matrix<double> Camera2DistortionCoeffs { get; }
        Matrix<double> Camera2Rectification { get; }
        Matrix<double> Camera2Projection { get; }
        Matrix<double> DisparityToDepth { get; }
        RotationVector3D Rotation { get; }
        Matrix<double> Translation { get; }
        Matrix<double> Fundamental { get; }
        Matrix<double> Essential { get; }
        Rectangle Image1ROI { get; }
        Rectangle Image2ROI { get; }

        void Calibrate(VectorOfVectorOfPointF cornersPointsLeft, VectorOfVectorOfPointF cornersPointsRight,
            int innerCornersPerChessboardCols, int innerCornersPerChessboardRows, Size imageSize);
    }
}