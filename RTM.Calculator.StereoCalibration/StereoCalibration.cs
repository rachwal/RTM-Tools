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
        public Matrix<double> LeftCameraMatrix { get; private set; }
        public Matrix<double> LeftCameraDistortionCoeffs { get; private set; }
        public Matrix<double> LeftCameraProjection { get; private set; }
        public Matrix<double> LeftCameraRectification { get; private set; }

        public Rectangle LeftImageROI
        {
            get { return leftImageRoi; }
            private set { leftImageRoi = value; }
        }

        public Matrix<float> LeftMapX { get; private set; }
        public Matrix<float> LeftMapY { get; private set; }

        public Matrix<double> RightCameraMatrix { get; private set; }
        public Matrix<double> RightCameraDistortionCoeffs { get; private set; }
        public Matrix<double> RightCameraProjection { get; private set; }
        public Matrix<double> RightCameraRectification { get; private set; }
        public Matrix<float> RightMapX { get; private set; }
        public Matrix<float> RightMapY { get; private set; }

        public Matrix<double> Rotation { get; private set; }
        public Matrix<double> Translation { get; private set; }
        public Matrix<double> Fundamental { get; private set; }
        public Matrix<double> Essential { get; private set; }
        public Matrix<double> DisparityToDepth { get; private set; }

        public Rectangle RightImageROI
        {
            get { return rightImageRoi; }
            private set { rightImageRoi = value; }
        }

        private MCvPoint3D32f[][] modelPoints;
        private Rectangle rightImageRoi;
        private Rectangle leftImageRoi;

        public void Calibrate(VectorOfVectorOfPointF cornersPointsLeft, VectorOfVectorOfPointF cornersPointsRight,
            int innerCornersPerChessboardCols, int innerCornersPerChessboardRows, Size imageSize)
        {
            Initialize(cornersPointsLeft.Size, cornersPointsRight.Size, innerCornersPerChessboardCols,
                innerCornersPerChessboardRows, imageSize);

            CvInvoke.StereoCalibrate(modelPoints, cornersPointsLeft.ToArrayOfArray(),
                cornersPointsRight.ToArrayOfArray(), LeftCameraMatrix,
                LeftCameraDistortionCoeffs, RightCameraMatrix, RightCameraDistortionCoeffs, imageSize, Rotation,
                Translation,
                Essential, Fundamental, CalibType.Default, new MCvTermCriteria(0.1e5));

            CvInvoke.StereoRectify(LeftCameraMatrix, LeftCameraDistortionCoeffs,
                RightCameraMatrix, RightCameraDistortionCoeffs, imageSize,
                Rotation, Translation, LeftCameraRectification, RightCameraRectification,
                LeftCameraProjection, RightCameraProjection, DisparityToDepth, StereoRectifyType.Default, 0, imageSize,
                ref rightImageRoi,
                ref rightImageRoi);

            CvInvoke.InitUndistortRectifyMap(LeftCameraMatrix, LeftCameraDistortionCoeffs,
                null, LeftCameraMatrix, imageSize, DepthType.Cv32F, LeftMapX, LeftMapY);

            CvInvoke.InitUndistortRectifyMap(RightCameraMatrix, RightCameraDistortionCoeffs,
                null, RightCameraMatrix, imageSize, DepthType.Cv32F, RightMapX, RightMapY);
        }

        private void Initialize(int cornersPointsLeft, int cornersPointsRight, int innerCornersPerChessboardCols,
            int innerCornersPerChessboardRows, Size imageSize)
        {
            if (cornersPointsLeft != cornersPointsRight)
            {
                throw new Exception(
                    $"cornersPointsLeft.Size: {cornersPointsLeft} should be equal to cornersPointsRight.Size: {cornersPointsRight}");
            }

            CreateModelPoints(cornersPointsLeft, innerCornersPerChessboardCols, innerCornersPerChessboardRows);

            LeftCameraDistortionCoeffs = new Matrix<double>(5, 1);
            LeftCameraMatrix = new Matrix<double>(3, 3);

            RightCameraDistortionCoeffs = new Matrix<double>(5, 1);
            RightCameraMatrix = new Matrix<double>(3, 3);

            Essential = new Matrix<double>(3, 3);
            Fundamental = new Matrix<double>(3, 3);

            Rotation = new Matrix<double>(3, 3);
            Translation = new Matrix<double>(3, 1);

            LeftCameraProjection = new Matrix<double>(3, 4);
            LeftCameraRectification = new Matrix<double>(3, 3);
            RightCameraProjection = new Matrix<double>(3, 4);
            RightCameraRectification = new Matrix<double>(3, 3);
            DisparityToDepth = new Matrix<double>(4, 4);

            LeftImageROI = new Rectangle(0, 0, imageSize.Width, imageSize.Height);
            RightImageROI = new Rectangle(0, 0, imageSize.Width, imageSize.Height);

            LeftMapX = new Matrix<float>(imageSize);
            LeftMapY = new Matrix<float>(imageSize);
            RightMapX = new Matrix<float>(imageSize);
            RightMapY = new Matrix<float>(imageSize);
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