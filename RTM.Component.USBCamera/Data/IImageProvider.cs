// RTM.Tools
// RTM.Component.USBCamera
// IImageProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component.USBCamera.Data
{
    public interface IImageProvider
    {
        CameraImage Image { get; set; }
        event EventHandler NewImage;
    }
}