// RTM.Tools
// RTM.Component.HarrisCornerDetector
// IHarrisDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component.HarrisCornerDetector.Detector
{
    public interface IHarrisDetector
    {
        CameraImage Image { get; set; }
        void Detect(CameraImage image);
        event EventHandler NewImage;
    }
}