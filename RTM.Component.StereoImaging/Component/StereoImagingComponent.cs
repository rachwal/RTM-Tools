// RTM.Tools
// RTM.Component.StereoImaging
// StereoImagingComponent.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

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
        [InPort(PortName = "leftCamera")] private readonly InPort<CameraImage> leftCameraIn = new InPort<CameraImage>();

        [OutPort(PortName = "leftCamera")] private readonly OutPort<CameraImage> leftCameraOut =
            new OutPort<CameraImage>();

        [InPort(PortName = "rightCamera")] private readonly InPort<CameraImage> rightCameraIn =
            new InPort<CameraImage>();

        [OutPort(PortName = "rightCamera")] private readonly OutPort<CameraImage> rightCameraOut =
            new OutPort<CameraImage>();

        [OutPort(PortName = "disparityMap")] private readonly OutPort<CameraImage> disparityMap =
            new OutPort<CameraImage>();

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
            }
        }

        private void OnNewDisparityMap(object sender, CameraImage cameraImage)
        {
            disparityMap.Write(cameraImage);
        }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            leftCameraIn.OnWrite += ProcessLeftCamera;
            rightCameraIn.OnWrite += ProcessRightCamera;
            return base.OnActivated(execHandle);
        }

        private void ProcessLeftCamera(CameraImage image)
        {
            leftCameraOut.Write(image);
            StereoImaging.ProcessLeftImage(image);
        }

        private void ProcessRightCamera(CameraImage image)
        {
            rightCameraOut.Write(image);
            StereoImaging.ProcessRightImage(image);
        }
        
        protected override ReturnCode_t OnDeactivated(int execHandle)
        {
            leftCameraIn.OnWrite -= ProcessLeftCamera;
            rightCameraIn.OnWrite -= ProcessRightCamera;
            return base.OnDeactivated(execHandle);
        }
    }
}