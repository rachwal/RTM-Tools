// RTM.Tools
// RTM.Component.SURFDetector
// IHomography.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public interface IHomography
    {
        Mat Calculate(Bitmap referenceBitmap, Bitmap currentBitmap);
    }
}