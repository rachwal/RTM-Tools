// RTM.Tools
// RTM.Component.SURFDetector
// SURFDetectorComponent.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.Component.CameraMovementDetector.Detector;

namespace RTM.Component.CameraMovementDetector.Component
{
    [Component(Category = "CameraMovement", Name = "CameraMovementDetectorComponent")]
    [DetailProfile(
        Description = "Camera Movement Detector Component",
        Language = "C#",
        LanguageType = "Compile",
        MaxInstance = 1,
        Vendor = "AIST",
        Version = "1.0.0")]
    [CustomProfile("CreationDate", "2015/08/11")]
    [CustomProfile("Author", "Bartosz Rachwal")]
    public class CameraMovementDetectorComponent : DataFlowComponent
    {
        [InPort(PortName = "in")] private readonly InPort<CameraImage> inport = new InPort<CameraImage>();
        [OutPort(PortName = "out")] private readonly OutPort<CameraImage> outport = new OutPort<CameraImage>();
        private IDetector detector;

        public IDetector Detector
        {
            get { return detector; }
            set
            {
                detector = value;
                if (value != null)
                {
                    Detector.NewImage += OnNewImage;
                }
            }
        }

        private void OnNewImage(object sender, EventArgs e)
        {
            outport.Write(Detector.Image);
        }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            inport.OnWrite += OnWrite;
            return base.OnActivated(execHandle);
        }

        private void OnWrite(CameraImage image)
        {
            Detector.Detect(image);
        }

        protected override ReturnCode_t OnDeactivated(int execHandle)
        {
            inport.OnWrite -= OnWrite;
            return base.OnDeactivated(execHandle);
        }
    }
}