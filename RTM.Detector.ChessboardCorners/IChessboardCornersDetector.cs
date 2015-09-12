// RTM.Tools
// RTM.Detector.ChessboardCorners
// IChessboardCornersDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace RTM.Detector.ChessboardCorners
{
    public interface IChessboardCornersDetector
    {
        VectorOfPointF Detect(Image<Gray, byte> image, int innerCornersPerChessboardCols, int innerCornersPerChessboardRows);
    }
}