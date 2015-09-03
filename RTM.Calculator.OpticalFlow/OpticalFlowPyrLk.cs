// RTM.Tools
// RTM.Calculator.OpticalFlow
// OpticalFlowPyrLk.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;

namespace RTM.Calculator.OpticalFlow
{
    public class OpticalFlowPyrLk : IOpticalFlow
    {
        private MCvTermCriteria criteria = new MCvTermCriteria(30, 0.001);
        private int level = 3;
        private Size winSize = new Size(10, 10);

        public void Initialize(int searchWindowSize, int maximalPyramidLevel, int maxIterations, double maxEps)
        {
            winSize = new Size(searchWindowSize, searchWindowSize);
            level = maximalPyramidLevel;
            criteria = new MCvTermCriteria(maxIterations, maxEps);
        }

        public PointF[] Calculate(IImage prev, IImage curr, MKeyPoint[] prevFeatures)
        {
            PointF[] features;
            byte[] status;
            float[] trackError;
            var points = prevFeatures.Select(e => e.Point).ToArray();
            CvInvoke.CalcOpticalFlowPyrLK(prev, curr, points, winSize, level, criteria, out features,
                out status, out trackError);
            return features;
        }
    }
}