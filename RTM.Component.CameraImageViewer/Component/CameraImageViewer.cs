// RTM.Tools
// RTM.Component.CameraImageViewer
// CameraImageViewer.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using OpenRTM.Core;
using RTM.Component.CameraImageViewer.ImageProvider;

namespace RTM.Component.CameraImageViewer.Component
{
    [Component(Category = "CameraImageViewer", Name = "CameraImageViewer")]
    [DetailProfile(
        Description = "CameraImage Viewer Component",
        Language = "C#",
        LanguageType = "Compile",
        MaxInstance = 1,
        Vendor = "AIST",
        Version = "1.0.0")]
    [CustomProfile("CreationDate", "2015/08/7")]
    [CustomProfile("Author", "Bartosz Rachwal")]
    public class CameraImageViewer : DataFlowComponent
    {
        [InPort(PortName = "in")] private readonly InPort<CameraImage> inport = new InPort<CameraImage>();

        public IImageProvider ImageProvider { get; set; }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            inport.OnWrite += OnWrite;
            return base.OnActivated(execHandle);
        }

        private void OnWrite(CameraImage image)
        {
            ImageProvider.SetImage(image);
        }

        protected override ReturnCode_t OnDeactivated(int execHandle)
        {
            inport.OnWrite -= OnWrite;
            return base.OnDeactivated(execHandle);
        }
    }
}