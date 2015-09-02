// RTM.Tools
// RTM.Component.CameraStabilizer
// CameraStabilizer.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component.CameraStabilizer.Stabilizer
{
    public class CameraStabilizer : ICameraStabilizer
    {
        private CameraImage image;

        public CameraImage Image
        {
            get { return image; }
            set
            {
                image = value;
                NewImage?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler NewImage;

        public void ProcessImage(CameraImage cameraImage)
        {
            Image = cameraImage;
        }
    }
}