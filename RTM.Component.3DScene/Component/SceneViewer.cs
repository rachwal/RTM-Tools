// RTM.Tools
// RTM.Component.3DScene
// SceneViewer.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using OpenRTM.Core;
using RTM.Component._3DScene.DataProvider;
using RTM.DTO;

namespace RTM.Component._3DScene.Component
{
    [Component(Category = "SceneViewer", Name = "SceneViewer")]
    [DetailProfile(
        Description = "Scene Viewer Component",
        Language = "C#",
        LanguageType = "Compile",
        MaxInstance = 1,
        Vendor = "AIST",
        Version = "1.0.0")]
    [CustomProfile("CreationDate", "2015/08/7")]
    [CustomProfile("Author", "Bartosz Rachwal")]
    public class SceneViewer : DataFlowComponent
    {
        [InPort(PortName = "in")] private readonly InPort<Quadrilateral> inport = new InPort<Quadrilateral>();

        public IDataProvider DataProvider { get; set; }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            inport.OnWrite += OnWrite;
            return base.OnActivated(execHandle);
        }

        private void OnWrite(Quadrilateral vector)
        {
            DataProvider.Quadrilateral = vector;
        }

        protected override ReturnCode_t OnDeactivated(int execHandle)
        {
            inport.OnWrite -= OnWrite;
            return base.OnDeactivated(execHandle);
        }
    }
}