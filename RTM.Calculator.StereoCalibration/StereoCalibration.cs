// RTM.Tools
// RTM.Calculator.StereoCalibration
// StereoCalibration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace RTM.Calculator.StereoCalibration
{
    public class StereoCalibration : IStereoCalibration
    {
        private Matrix<double> camera1DistortionCoeffs = new Matrix<double>(5, 1);
        private Matrix<double> camera1Matrix = new Matrix<double>(3, 3);
        private Matrix<double> camera1Projection = new Matrix<double>(3, 4);
        private Matrix<double> camera1Rectification = new Matrix<double>(3, 3);

        private Matrix<double> camera2DistortionCoeffs = new Matrix<double>(5, 1);
        private Matrix<double> camera2Matrix = new Matrix<double>(3, 3);
        private Matrix<double> camera2Projection = new Matrix<double>(3, 4);
        private Matrix<double> camera2Rectification = new Matrix<double>(3, 3);

        private Matrix<double> disparityToDepth = new Matrix<double>(4, 4);
        private Matrix<double> essential = new Matrix<double>(3, 3);

        private Matrix<double> fundamental = new Matrix<double>(3, 3);
       
        private Rectangle image1ROI;
        private Rectangle image2ROI;

        private MCvPoint3D32f[][] modelPoints;

        private RotationVector3D rotation = new RotationVector3D();
        private Matrix<double> translation = new Matrix<double>(3, 1);

        public Matrix<double> Camera1Matrix => camera1Matrix;
        public Matrix<double> Camera1DistortionCoeffs => camera1DistortionCoeffs;
        public Matrix<double> Camera1Rectification => camera1Rectification;
        public Matrix<double> Camera1Projection => camera1Projection;

        public Matrix<double> Camera2Matrix => camera2Matrix;
        public Matrix<double> Camera2DistortionCoeffs => camera2DistortionCoeffs;
        public Matrix<double> Camera2Rectification => camera2Rectification;
        public Matrix<double> Camera2Projection => camera2Projection;

        public Matrix<double> DisparityToDepth => disparityToDepth;
        public RotationVector3D Rotation => rotation;
        public Matrix<double> Translation => translation;
        public Matrix<double> Fundamental => fundamental;
        public Matrix<double> Essential => essential;
        public Rectangle Image1ROI => image1ROI;
        public Rectangle Image2ROI => image2ROI;
        
        public void Calibrate(VectorOfVectorOfPointF cornersPointsLeft, VectorOfVectorOfPointF cornersPointsRight, int innerCornersPerChessboardCols, int innerCornersPerChessboardRows, Size imageSize)
        {
            if (cornersPointsLeft.Size != cornersPointsRight.Size)
            {
                throw new Exception(
                    $"cornersPointsLeft.Size: {cornersPointsLeft.Size} should be equal to cornersPointsRight.Size: {cornersPointsRight.Size}");
            }

            CreateModelPoints(cornersPointsLeft.Size, innerCornersPerChessboardCols, innerCornersPerChessboardRows);

            CvInvoke.StereoCalibrate(modelPoints, cornersPointsLeft.ToArrayOfArray(),
                cornersPointsRight.ToArrayOfArray(), camera1Matrix,
                camera1DistortionCoeffs, camera2Matrix, camera2DistortionCoeffs, imageSize, rotation, translation,
                essential, fundamental, CalibType.Default, new MCvTermCriteria(0.1e5));

            CvInvoke.StereoRectify(camera1Matrix, camera1DistortionCoeffs, camera2Matrix, camera2DistortionCoeffs,
                imageSize, rotation, translation, camera1Rectification, camera2Rectification, camera1Projection,
                camera2Projection, disparityToDepth, StereoRectifyType.Default, 0, imageSize, ref image1ROI,
                ref image2ROI);
        }

        private void CreateModelPoints(int length, int innerCornersPerChessboardCols, int innerCornersPerChessboardRows)
        {
            modelPoints = new MCvPoint3D32f[length][];
            for (var k = 0; k < length; k++)
            {
                var chessboard = new List<MCvPoint3D32f>();
                for (var y = 0; y < innerCornersPerChessboardRows; y++)
                {
                    for (var x = 0; x < innerCornersPerChessboardCols; x++)
                    {
                        chessboard.Add(new MCvPoint3D32f(x, y, 0));
                    }
                }
                modelPoints[k] = chessboard.ToArray();
            }
        }
    }
}