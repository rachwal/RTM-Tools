// RTM.Tools
// RTM.Calculator.CameraCalibration
// ICameraCalibration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Util;

namespace RTM.Calculator.CameraCalibration
{
    public interface ICameraCalibration
    {
        Matrix<double> CameraMatrix { get; }
        Matrix<double> CameraDistortionCoeffs { get; }
        RotationVector3D Rotation { get; }
        Matrix<double> Translation { get; }

        void Calibrate(VectorOfVectorOfPointF cornersPoints, Size imageSize, int innerCornersPerChessboardCols, int innerCornersPerChessboardRows);
    }
}