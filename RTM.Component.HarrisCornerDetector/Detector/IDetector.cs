// RTM.Tools
// RTM.Component.HarrisCornerDetector
// IDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component.HarrisCornerDetector.Detector
{
    public interface IDetector
    {
        void Detect(CameraImage image);
        event EventHandler NewImage;
        CameraImage Image { get; set; }
    }
}