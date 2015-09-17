// RTM.Tools
// RTM.Component.CameraMovementDetector
// CameraMovementDetectorComponent.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.Component.CameraMovementDetector.MovementDetector;
using RTM.DTO;

namespace RTM.Component.CameraMovementDetector.Component
{
    [Component(Category = "CameraMovement", Name = "CameraMovementDetector")]
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
        [InPort(PortName = "camera")] private readonly InPort<CameraImage> inport = new InPort<CameraImage>();

        [OutPort(PortName = "camera")] private readonly OutPort<CameraImage> outportCamera = new OutPort<CameraImage>();

        [OutPort(PortName = "vectors")] private readonly OutPort<Vectors> outportVectors = new OutPort<Vectors>();

        private ICameraMovementDetector cameraMovementDetector;

        public ICameraMovementDetector CameraMovementDetector
        {
            get { return cameraMovementDetector; }
            set
            {
                cameraMovementDetector = value;
                if (value == null)
                {
                    return;
                }
                CameraMovementDetector.NewImage += OnNewImage;
                cameraMovementDetector.NewVectors += OnNewVectors;
            }
        }

        private void OnNewVectors(object sender, EventArgs e)
        {
            outportVectors.Write(CameraMovementDetector.Vectors);
        }

        private void OnNewImage(object sender, EventArgs e)
        {
            outportCamera.Write(CameraMovementDetector.Image);
        }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            inport.OnWrite += OnWrite;
            return base.OnActivated(execHandle);
        }

        private void OnWrite(CameraImage image)
        {
            CameraMovementDetector.ProcessImage(image);
        }

        protected override ReturnCode_t OnDeactivated(int execHandle)
        {
            inport.OnWrite -= OnWrite;
            return base.OnDeactivated(execHandle);
        }
    }
}