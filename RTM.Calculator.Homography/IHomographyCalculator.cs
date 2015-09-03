// RTM.Tools
// RTM.Calculator.Homography
// IHomographyCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;

namespace RTM.Calculator.Homography
{
    public interface IHomographyCalculator
    {
        Mat Calculate(Bitmap referenceBitmap, Bitmap currentBitmap);
        void Initialize(float uniquenessThreshold, int k);
    }
}