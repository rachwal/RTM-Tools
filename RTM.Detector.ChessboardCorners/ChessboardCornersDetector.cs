// RTM.Tools
// RTM.Detector.ChessboardCorners
// ChessboardCornersDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace RTM.Detector.ChessboardCorners
{
    public class ChessboardCornersDetector : IChessboardCornersDetector
    {
        public VectorOfPointF Detect(Image<Gray, byte> image, int innerCornersPerChessboardCols, int innerCornersPerChessboardRows)
        {
            var corners = new VectorOfPointF();

            CvInvoke.FindChessboardCorners(image, new Size(innerCornersPerChessboardCols, innerCornersPerChessboardRows), corners);

            if (corners.Size != innerCornersPerChessboardCols * innerCornersPerChessboardRows)
            {
                return new VectorOfPointF(new[] {new PointF(0, 0)});
            }

            var refinedCorners = new[] {corners.ToArray()};

            image.FindCornerSubPix(refinedCorners, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(10));

            return new VectorOfPointF(refinedCorners[0]);
        }
    }
}