// RTM.Tools
// RTM.Calculator.OpticalFlow
// IOpticalFlow.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace RTM.Calculator.OpticalFlow
{
    public interface IOpticalFlow
    {
        void Initialize(int searchWindowSize, int maximalPyramidLevel, int maxIterations, double maxEps);
        PointF[] Calculate(IImage prev, IImage curr, MKeyPoint[] prevFeatures);
    }
}