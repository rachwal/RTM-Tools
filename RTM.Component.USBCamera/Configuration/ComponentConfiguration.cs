// RTM.Tools
// RTM.Component.USBCamera
// ComponentConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;

namespace RTM.Component.USBCamera.Configuration
{
    public class ComponentConfiguration : IComponentConfiguration
    {
        private int cameraIndex;
        private bool running;

        public int CameraIndex
        {
            get { return cameraIndex; }
            set
            {
                if (value == cameraIndex)
                {
                    return;
                }
                cameraIndex = value;
                CameraIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler CameraIndexChanged;

        public bool Running
        {
            get { return running; }
            set
            {
                if (value == running)
                {
                    return;
                }
                running = value;
                RunningChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler RunningChanged;

        public void Initialize()
        {
            CameraIndex = 0;
            Running = false;
        }
    }
}