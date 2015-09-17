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
        Matrix<double> LeftCameraMatrix { get; }
        Matrix<double> LeftCameraDistortionCoeffs { get; }
        Matrix<double> LeftCameraProjection { get; }
        Matrix<double> LeftCameraRectification { get; }
        Rectangle LeftImageROI { get; }
        Matrix<float> LeftMapX { get; }
        Matrix<float> LeftMapY { get; }

        Matrix<double> RightCameraMatrix { get; }
        Matrix<double> RightCameraDistortionCoeffs { get; }
        Matrix<double> RightCameraProjection { get; }
        Matrix<double> RightCameraRectification { get; }
        Matrix<float> RightMapX { get; }
        Matrix<float> RightMapY { get; }

        Matrix<double> Rotation { get; }
        Matrix<double> Translation { get; }
        Matrix<double> Fundamental { get; }
        Matrix<double> Essential { get; }
        Matrix<double> DisparityToDepth { get; }
        Rectangle RightImageROI { get; }

        void Calibrate(VectorOfVectorOfPointF cornersPointsLeft, VectorOfVectorOfPointF cornersPointsRight,
            int innerCornersPerChessboardCols, int innerCornersPerChessboardRows, Size imageSize);
    }
}