// RTM.Tools
// RTM.Component.StereoImaging
// IStereoImaging.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component.StereoImaging.Stereo
{
    public interface IStereoImaging
    {
        CameraImage DisparityMap { get; set; }
        CameraImage LeftCameraImage { get; set; }
        CameraImage RightCameraImage { get; set; }

        event EventHandler NewDisparityMap;

        event EventHandler NewRightCameraImage;

        event EventHandler NewLeftCameraImage;

        void ProcessRightImage(CameraImage image);
        void ProcessLeftImage(CameraImage image);
    }
}