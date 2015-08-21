// RTM.Tools
// RTM.Component.CameraMovementDetector
// CameraMovementDetectorComponent.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.Component.CameraMovementDetector.Detector;
using RTM.DTO;

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

        [OutPort(PortName = "out")] private readonly OutPort<CameraImage> outportCamera =
            new OutPort<CameraImage>();

        [OutPort(PortName = "quadrilateral")] private readonly OutPort<Quadrilateral> outportTranslation =
            new OutPort<Quadrilateral>();

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
                CameraMovementDetector.NewTranslationVector += OnNewTranslationVector;
            }
        }

        private void OnNewTranslationVector(object sender, EventArgs e)
        {
            outportTranslation.Write(CameraMovementDetector.Quadrilateral);
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