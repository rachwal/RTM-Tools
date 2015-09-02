// RTM.Tools
// RTM.Component.CameraMovementDetector
// OpticalFlowPyrLk.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public class OpticalFlowPyrLk : IOpticalFlow
    {
        private readonly Size winSize = new Size(10, 10);
        private int level = 3;
        private readonly MCvTermCriteria criteria = new MCvTermCriteria(30, 0.001);

        public PointF[] Calculate(IInputArray prev, IInputArray curr, IReadOnlyList<PointF[]> prevFeatures)
        {
            PointF[] currFeatures;
            byte[] status;
            float[] trackError;
            CvInvoke.CalcOpticalFlowPyrLK(prev, curr, prevFeatures[0], winSize, level, criteria, out currFeatures,
                out status, out trackError);
            return currFeatures;
        }
    }
}