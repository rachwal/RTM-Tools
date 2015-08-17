// RTM.Tools
// RTM.Component.SURFDetector
// IDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public interface IDetector
    {
        CameraImage Image { get; set; }
        void Detect(CameraImage image);
        event EventHandler NewImage;
    }
}