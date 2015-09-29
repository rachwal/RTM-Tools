// RTM.Tools
// RTM.Component.USBCamera
// IComponentConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;

namespace RTM.Component.USBCamera.Configuration
{
    public interface IComponentConfiguration
    {
        int CameraIndex { get; set; }
        event EventHandler CameraIndexChanged;
        bool Running { get; set; }
        event EventHandler RunningChanged;
        void Initialize();
    }
}