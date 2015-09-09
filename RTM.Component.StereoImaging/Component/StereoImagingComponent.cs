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
        private readonly InPort<CameraImage> camera1 = new InPort<CameraImage>();
        [InPort(PortName = "camera2")]
        private readonly InPort<CameraImage> camera2 = new InPort<CameraImage>();

        [OutPort(PortName = "out")] private readonly OutPort<CameraImage> outport = new OutPort<CameraImage>();

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
                StereoImaging.NewImage += OnNewImage;
            }
        }

        private void OnNewImage(object sender, EventArgs e)
        {
            outport.Write(StereoImaging.Image);
        }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            camera1.OnWrite += ProcessCamera1;
            camera2.OnWrite += ProcessCamera2;
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
            camera1.OnWrite -= ProcessCamera1;
            camera2.OnWrite -= ProcessCamera2;
            return base.OnDeactivated(execHandle);
        }
    }
}