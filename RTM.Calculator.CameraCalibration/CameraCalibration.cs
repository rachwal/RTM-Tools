// RTM.Tools
// RTM.Calculator.CameraCalibration
// CameraCalibration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace RTM.Calculator.CameraCalibration
{
    public class CameraCalibration : ICameraCalibration
    {
        public Matrix<double> CameraMatrix => cameraMatrix;
        public Matrix<double> CameraDistortionCoeffs => cameraDistortionCoeffs;
        public RotationVector3D Rotation => rotation;
        public Matrix<double> Translation => translation;

        private Matrix<double> cameraDistortionCoeffs = new Matrix<double>(5, 1);
        private Matrix<double> cameraMatrix = new Matrix<double>(3, 3);
        private RotationVector3D rotation = new RotationVector3D();
        private Matrix<double> translation = new Matrix<double>(3, 1);

        private VectorOfVectorOfPoint3D32F modelPoints;

        public void Calibrate(VectorOfVectorOfPointF cornersPoints, Size imageSize, int innerCornersPerChessboardCols,
            int innerCornersPerChessboardRows)
        {
            modelPoints = CreateModelPoints(cornersPoints.Size, innerCornersPerChessboardCols,
                innerCornersPerChessboardRows);

            var rotationVectors = new VectorOfMat();
            var translationVectors = new VectorOfMat();

            CvInvoke.CalibrateCamera(modelPoints, cornersPoints, imageSize, cameraMatrix, cameraDistortionCoeffs,
                rotationVectors, translationVectors, CalibType.Default, new MCvTermCriteria(10));

            translation = new Matrix<double>(translationVectors[0].Rows, translationVectors[0].Cols,
                translationVectors[0].DataPointer);

            var rotationMatrix = new Matrix<double>(rotationVectors[0].Rows, rotationVectors[0].Cols,
                rotationVectors[0].DataPointer);

            rotation = new RotationVector3D(new[] {rotationMatrix[0, 0], rotationMatrix[1, 0], rotationMatrix[2, 0]});
        }

        private VectorOfVectorOfPoint3D32F CreateModelPoints(int length, int chessboardCols, int chessboardRows)
        {
            modelPoints = new VectorOfVectorOfPoint3D32F();
            for (var k = 0; k < length; k++)
            {
                var chessboard = new List<MCvPoint3D32f>();
                for (var y = 0; y < chessboardRows; y++)
                {
                    for (var x = 0; x < chessboardCols; x++)
                    {
                        chessboard.Add(new MCvPoint3D32f(x, y, 0));
                    }
                }
                modelPoints.Push(new VectorOfPoint3D32F(chessboard.ToArray()));
            }
            return modelPoints;
        }
    }
}