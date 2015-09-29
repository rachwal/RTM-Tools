// RTM.Tools
// RTM.Component.USBCamera
// USBCamera.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using OpenRTM.Extension;
using RTM.Component.USBCamera.Configuration;
using RTM.Component.USBCamera.Data;
using RTM.Component.USBCamera.Device;

namespace RTM.Component.USBCamera.Component
{
    [Component(Category = "USBCamera", Name = "USBCamera")]
    [DetailProfile(
        Description = "USBCamera Component",
        Language = "C#",
        LanguageType = "Compile",
        MaxInstance = 1,
        Vendor = "AIST",
        Version = "1.0.0")]
    [CustomProfile("CreationDate", "2015/09/28")]
    [CustomProfile("Author", "Bartosz Rachwal")]
    public class USBCamera : DataFlowComponent
    {
        [OutPort(PortName = "out")] private readonly OutPort<CameraImage> outport = new OutPort<CameraImage>();

        public IComponentConfiguration Configuration { get; set; }
        public IImageProvider Provider { get; set; }
        public ICameraDevice Camera { get; set; }

        private ConfigurationSet configurationSet;

        public USBCamera()
        {
            Config.OnSetConfigurationSet += OnSetConfigurationSet;
        }

        [Configuration(DefaultValue = "0", Name = "CameraIndex")]
        public int CameraIndex
        {
            get { return Configuration?.CameraIndex ?? 0; }
            set
            {
                if (Configuration == null)
                {
                    return;
                }
                Configuration.CameraIndex = value;
            }
        }

        private void OnNewImage(object sender, EventArgs e)
        {
            outport.Write(Provider.Image);
        }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            Provider.NewImage += OnNewImage;
            Configuration.Running = true;
            return base.OnActivated(execHandle);
        }

        protected override ReturnCode_t OnDeactivated(int execHandle)
        {
            Configuration.Running = false;
            Provider.NewImage -= OnNewImage;
            return base.OnDeactivated(execHandle);
        }

        private void OnSetConfigurationSet(ConfigurationSet obj)
        {
            if (obj == null)
                return;

            if (configurationSet == null)
            {
                configurationSet = obj;
                return;
            }

            if (obj.Id != configurationSet.Id)
                return;

            foreach (var entry in obj.ConfigurationData)
            {
                if (configurationSet.ConfigurationData.ContainsKey(entry.Key))
                {
                    configurationSet.ConfigurationData[entry.Key] = entry.Value;
                }
                else
                {
                    configurationSet.ConfigurationData.Add(entry.Key, entry.Value);
                }
            }

            Config.OnSetConfigurationSet -= OnSetConfigurationSet;

            foreach (var entry in configurationSet.ConfigurationData)
            {
                this.AddConfigurationValue(entry.Key, entry.Value.ToString());
            }

            Config.OnSetConfigurationSet += OnSetConfigurationSet;
        }
    }
}