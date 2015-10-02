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
        event EventHandler<CameraImage> NewDisparityMap;

        void ProcessRightImage(CameraImage image);
        void ProcessLeftImage(CameraImage image);
    }
}