// RTM.Tools
// RTM.Component.CameraStabilizer
// ICameraStabilizer.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component.CameraStabilizer.Stabilizer
{
    public interface ICameraStabilizer
    {
        CameraImage Image { get; set; }

        event EventHandler NewImage;
        void ProcessImage(CameraImage image);
    }
}