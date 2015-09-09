// RTM.Tools
// RTM.Component.StereoImaging
// StereoImaging.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OpenRTM.Core;
using RTM.Converter.CameraImage;

// reference
// http://www.emgu.com/wiki/index.php/Stereo_Imaging

namespace RTM.Component.StereoImaging.Stereo
{
    public class StereoImaging : IStereoImaging
    {
        private readonly ICameraImageConverter converter;
     
        private CameraImage image;

        public CameraImage Image
        {
            get { return image; }
            set
            {
                image = value;
                NewImage?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler NewImage;

        private volatile CameraImage cameraImage1;
        private volatile CameraImage cameraImage2;

        private Image<Bgr, byte> frameS1;
        private Image<Gray, byte> grayFrameS1;
        private Image<Bgr, byte> frameS2;
        private Image<Gray, byte> grayFrameS2;

        private const int Width = 9;
        private const int Height = 6;
        private Size patternSize = new Size(Width, Height);
        private VectorOfPointF cornersLeft = new VectorOfPointF();
        private VectorOfPointF cornersRight = new VectorOfPointF();

        private static int BufferLength = 1;
        private int bufferSavepoint = 0;

        private MCvPoint3D32f[][] cornersObjectPoints = new MCvPoint3D32f[BufferLength][];
        private PointF[][] cornersPointsLeft = new PointF[BufferLength][];
        private PointF[][] cornersPointsRight = new PointF[BufferLength][];

        private Matrix<double> camera1Matrix = new Matrix<double>(3, 3);
        private Matrix<double> distortion1Coeffs = new Matrix<double>(5, 1);

        private Matrix<double> camera2Matrix = new Matrix<double>(3, 3);
        private Matrix<double> distortion2Coeffs = new Matrix<double>(5, 1);

        private RotationVector3D r = new RotationVector3D();
        private Matrix<double> t = new Matrix<double>(3, 1);

        private Matrix<double> fundamental = new Matrix<double>(3, 3);
        private Matrix<double> essential = new Matrix<double>(3, 3);

        private Rectangle rec1;
        private Rectangle rec2;

        private Matrix<double> q = new Matrix<double>(4, 4);
        private Matrix<double> r1 = new Matrix<double>(3, 3);
        private Matrix<double> r2 = new Matrix<double>(3, 3);
        private Matrix<double> p1 = new Matrix<double>(3, 4);
        private Matrix<double> p2 = new Matrix<double>(3, 4);
        private MCvPoint3D32f[] points;

        public StereoImaging(ICameraImageConverter cameraImageConverter)
        {
            converter = cameraImageConverter;
        }

        public void ProcessImage2(CameraImage cameraImage)
        {
            if (cameraImage2 == null)
            {
                cameraImage2 = cameraImage.Copy();
                return;
            }
            Process();
        }

        public void ProcessImage1(CameraImage cameraImage)
        {
            if (cameraImage1 == null)
            {
                cameraImage1 = cameraImage.Copy();
                return;
            }
            Process();
        }
        
        public enum Mode
        {
            CaluculatingStereoIntrinsics,
            Calibrated,
            SavingFrames
        }

        Mode currentMode = Mode.SavingFrames;

        private volatile bool busy;

        private void Process()
        {
            if (cameraImage1 == null || cameraImage2 == null)
            {
                return;
            }

            if (!busy)
            {
                var bitmap1 = converter.ToBitmap(cameraImage1);
                cameraImage1 = null;
                var bitmap2 = converter.ToBitmap(cameraImage2);
                cameraImage2 = null;

                frameS1 = new Image<Bgr, byte>(bitmap1);
                frameS2 = new Image<Bgr, byte>(bitmap2);
                grayFrameS1 = new Image<Gray, byte>(bitmap1);
                grayFrameS2 = new Image<Gray, byte>(bitmap2);

            }

            if (currentMode == Mode.SavingFrames)
            {
                CvInvoke.FindChessboardCorners(grayFrameS1, patternSize, cornersLeft, CalibCbType.AdaptiveThresh);
                CvInvoke.FindChessboardCorners(grayFrameS2, patternSize, cornersRight, CalibCbType.AdaptiveThresh);

                if (cornersLeft.Size != 54 || cornersRight.Size != 54)
                {
                    busy = false;
                    return;
                }

                if (cornersLeft != null && cornersRight != null)
                {
                    var refinedCornersLeft = new[] { cornersLeft.ToArray() };
                    var refinedCornersRight = new[] { cornersRight.ToArray() };

                    grayFrameS1.FindCornerSubPix(refinedCornersLeft, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.01));
                    grayFrameS2.FindCornerSubPix(refinedCornersRight, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.01));

                    cornersPointsLeft[bufferSavepoint] = refinedCornersLeft[0];
                    cornersPointsRight[bufferSavepoint] = refinedCornersRight[0];

                    currentMode = Mode.CaluculatingStereoIntrinsics;

                    Thread.Sleep(100);
                }
            }

            if (currentMode == Mode.CaluculatingStereoIntrinsics)
            {
                for (var k = 0; k < 1; k++)
                {
                    var objectList = new List<MCvPoint3D32f>();
                    for (var i = 0; i < Height; i++)
                    {
                        for (var j = 0; j < Width; j++)
                        {
                            objectList.Add(new MCvPoint3D32f(j * 20.0F, i * 20.0F, 0.0F));
                        }
                    }
                    cornersObjectPoints[k] = objectList.ToArray();
                }

                CvInvoke.StereoCalibrate(cornersObjectPoints, cornersPointsLeft, cornersPointsRight, camera1Matrix, distortion1Coeffs, camera2Matrix, distortion2Coeffs, frameS1.Size, r, t, essential, fundamental, CalibType.Default, new MCvTermCriteria(0.1e5));

                CvInvoke.StereoRectify(camera1Matrix, distortion1Coeffs, camera2Matrix, distortion2Coeffs, frameS1.Size, r, t, r1, r2, p1, p2, q, StereoRectifyType.Default, 0, frameS1.Size, ref rec1, ref rec2);

                currentMode = Mode.Calibrated;

            }

            if (currentMode == Mode.Calibrated)
            {
                Image<Gray, short> disparityMap;

                Computer3DPointsFromStereoPair(grayFrameS1, grayFrameS2, out disparityMap, out points);

                Image = converter.Convert(disparityMap.ToBitmap());

                busy = false;
            }
        }

        private void Computer3DPointsFromStereoPair(Image<Gray, byte> left, Image<Gray, byte> right, out Image<Gray, short> disparityMap, out MCvPoint3D32f[] points)
        {
            var size = left.Size;

            disparityMap = new Image<Gray, short>(size);

            var numDisparities = 64;
            var minDispatities = 0;
            var sad = 1;
            var p1 = 8 * 1 * sad * sad;
            var p2 = 32 * 1 * sad * sad;
            var disp12MaxDiff = -1;
            var preFilterCap = 0;
            var uniquenessRatio = 10;
            var speckle = 16;
            var speckleRange = 16;

            using (var stereoSolver = new StereoSGBM(minDispatities, numDisparities, sad, p1, p2, disp12MaxDiff, preFilterCap, uniquenessRatio, speckle, speckleRange))
            {
                stereoSolver.Compute(left, right, disparityMap);
                points = PointCollection.ReprojectImageTo3D(disparityMap, q);
            }
        }
    }
}