// RTM.Tools
// RTM.Component.CameraImageViewer
// IImageProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.Images.Factory;

namespace RTM.Component.CameraImageViewer.ImageProvider
{
    public interface IImageProvider
    {
        Image Image { get; }
        event EventHandler NewImage;
        void SetImage(CameraImage cameraImage);
    }
}