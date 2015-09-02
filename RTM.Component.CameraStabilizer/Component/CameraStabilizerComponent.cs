// RTM.Tools
// RTM.Component.CameraStabilizer
// CameraStabilizerComponent.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.Component.CameraStabilizer.Stabilizer;

namespace RTM.Component.CameraStabilizer.Component
{
    [Component(Category = "CameraStabilizer", Name = "CameraStabilizer")]
    [DetailProfile(
        Description = "Camera Stabilizer Component",
        Language = "C#",
        LanguageType = "Compile",
        MaxInstance = 1,
        Vendor = "AIST",
        Version = "1.0.0")]
    [CustomProfile("CreationDate", "2015/08/31")]
    [CustomProfile("Author", "Bartosz Rachwal")]
    public class CameraStabilizerComponent : DataFlowComponent
    {
        [InPort(PortName = "in")] private readonly InPort<CameraImage> inport = new InPort<CameraImage>();

        [OutPort(PortName = "out")] private readonly OutPort<CameraImage> outport = new OutPort<CameraImage>();

        private ICameraStabilizer cameraStabilizer;

        public ICameraStabilizer CameraStabilizer
        {
            get { return cameraStabilizer; }
            set
            {
                cameraStabilizer = value;
                if (value == null)
                {
                    return;
                }
                CameraStabilizer.NewImage += OnNewImage;
            }
        }

        private void OnNewImage(object sender, EventArgs e)
        {
            outport.Write(CameraStabilizer.Image);
        }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            inport.OnWrite += OnWrite;
            return base.OnActivated(execHandle);
        }

        private void OnWrite(CameraImage image)
        {
            CameraStabilizer.ProcessImage(image);
        }

        protected override ReturnCode_t OnDeactivated(int execHandle)
        {
            inport.OnWrite -= OnWrite;
            return base.OnDeactivated(execHandle);
        }
    }
}