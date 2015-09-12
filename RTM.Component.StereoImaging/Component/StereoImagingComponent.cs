// RTM.Tools
// RTM.Component.StereoImaging
// StereoImagingComponent.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.Component.StereoImaging.Stereo;

namespace RTM.Component.StereoImaging.Component
{
    [Component(Category = "StereoImaging", Name = "StereoImaging")]
    [DetailProfile(
        Description = "Camera Stereo Imaging Component",
        Language = "C#",
        LanguageType = "Compile",
        MaxInstance = 1,
        Vendor = "AIST",
        Version = "1.0.0")]
    [CustomProfile("CreationDate", "2015/08/31")]
    [CustomProfile("Author", "Bartosz Rachwal")]
    public class StereoImagingComponent : DataFlowComponent
    {
        [InPort(PortName = "camera1")]
        private readonly InPort<CameraImage> camera1In = new InPort<CameraImage>();

        [OutPort(PortName = "camera1")]
        private readonly OutPort<CameraImage> camera1Out = new OutPort<CameraImage>();

        [InPort(PortName = "camera2")]
        private readonly InPort<CameraImage> camera2In = new InPort<CameraImage>();

        [OutPort(PortName = "camera2")]
        private readonly OutPort<CameraImage> camera2Out = new OutPort<CameraImage>();

        [OutPort(PortName = "disparityMap")]
        private readonly OutPort<CameraImage> disparityMap = new OutPort<CameraImage>();

        private IStereoImaging stereoImaging;

        public IStereoImaging StereoImaging
        {
            get { return stereoImaging; }
            set
            {
                stereoImaging = value;
                if (value == null)
                {
                    return;
                }
                StereoImaging.NewDisparityMap += OnNewDisparityMap;
                StereoImaging.NewCamera1Image += OnNewCamera1Image;
                stereoImaging.NewCamera2Image += OnNewCamera2Image;
            }
        }

        private void OnNewCamera2Image(object sender, EventArgs e)
        {
            camera2Out.Write(StereoImaging.Camera2Image);
        }

        private void OnNewCamera1Image(object sender, EventArgs e)
        {
            camera1Out.Write(StereoImaging.Camera1Image);
        }

        private void OnNewDisparityMap(object sender, EventArgs e)
        {
            disparityMap.Write(StereoImaging.DisparityMap);
        }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            camera1In.OnWrite += ProcessCamera1;
            camera2In.OnWrite += ProcessCamera2;
            return base.OnActivated(execHandle);
        }

        private void ProcessCamera2(CameraImage image)
        {
            StereoImaging.ProcessImage1(image);
        }

        private void ProcessCamera1(CameraImage image)
        {
            StereoImaging.ProcessImage2(image);
        }

        protected override ReturnCode_t OnDeactivated(int execHandle)
        {
            camera1In.OnWrite -= ProcessCamera1;
            camera2In.OnWrite -= ProcessCamera2;
            return base.OnDeactivated(execHandle);
        }
    }
}