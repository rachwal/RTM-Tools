// RTM.Tools
// RTM.Component.CameraMovementDetector
// ChessboardCornersDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using RTM.Component.CameraMovementDetector.Configuration;

namespace RTM.Component.CameraMovementDetector.CornersDetector
{
    public class ChessboardCornersDetector : ICornersDetector
    {
        private readonly IComponentConfiguration configuration;

        public ChessboardCornersDetector(IComponentConfiguration componentConfiguration)
        {
            configuration = componentConfiguration;
        }

        private int Width => configuration.InnerCornersPerChessboardCols;
        private int Height => configuration.InnerCornersPerChessboardRows;

        public bool MatchFound(VectorOfVectorOfPointF corners)
        {
            return corners[0].Size == Width*Height;
        }

        public VectorOfVectorOfPointF Detect(Bitmap bitmap)
        {
            var curr = new Image<Gray, byte>(bitmap);
            var corners = new VectorOfPointF();

            CvInvoke.FindChessboardCorners(curr, new Size(Width, Height), corners);

            if (corners.Size != Width*Height)
            {
                return new VectorOfVectorOfPointF(new VectorOfPointF(new[] {new PointF(0, 0)}));
            }

            var refinedCorners = new[] {corners.ToArray()};

            curr.FindCornerSubPix(refinedCorners, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(10));

            return new VectorOfVectorOfPointF(refinedCorners);
        }
    }
}