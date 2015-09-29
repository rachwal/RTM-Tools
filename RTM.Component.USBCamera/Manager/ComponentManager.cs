// RTM.Tools
// RTM.Component.USBCamera
// ComponentManager.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Threading.Tasks;
using OpenRTM.Extension;
using OpenRTM.IIOP;
using RTM.Component.USBCamera.Configuration;
using RTM.Component.USBCamera.Data;
using RTM.Component.USBCamera.Device;

namespace RTM.Component.USBCamera.Manager
{
    public class ComponentManager : IComponentManager
    {
        private readonly IComponentConfiguration componentConfiguration;
        private readonly IImageProvider provider;
        private readonly ICameraDevice device;

        public ComponentManager(IComponentConfiguration configuration, IImageProvider imageProvider,
            ICameraDevice cameraDevice)
        {
            device = cameraDevice;
            provider = imageProvider;
            componentConfiguration = configuration;
        }

        public void Start(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                var manager = new OpenRTM.Core.Manager(args);
                manager.AddTypes(typeof (CorbaProtocolManager));
                manager.Activate();
                try
                {
                    var comp = manager.CreateComponent<Component.USBCamera>();
                    comp.Configuration = componentConfiguration;
                    comp.Camera = device;
                    comp.Provider = provider;
                    Console.WriteLine(comp.GetComponentProfile().Format());

                    manager.Run();
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine(@"Start naming service before running component");
                    Console.WriteLine(@"More info at http://www.openrtm.org/openrtm/en/node/1420");
                }
            });
        }
    }
}