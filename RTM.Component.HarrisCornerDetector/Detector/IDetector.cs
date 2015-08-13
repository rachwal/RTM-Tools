// RTM.Component.HarrisCornerDetector
// RTM.Component.HarrisCornerDetector
// IDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.Images.Factory;

namespace RTM.Component.HarrisCornerDetector.Detector
{
    public interface IDetector
    {
        void Detect(Image image);
        event EventHandler NewImage;
        CameraImage Image { get; set; }
    }
}