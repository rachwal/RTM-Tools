// RTM.Tools
// RTM.Component.USBCamera
// Program.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Microsoft.Practices.Unity;
using RTM.Component.USBCamera.Configuration;
using RTM.Component.USBCamera.Data;
using RTM.Component.USBCamera.Device;
using RTM.Component.USBCamera.Manager;
using RTM.Converter.CameraImage;

namespace RTM.Component.USBCamera
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RunComponent(args);
            WaitForExit();
        }

        private static void RunComponent(string[] args)
        {
            var container = new UnityContainer();

            container.RegisterType<IImageProvider, ImageProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICameraDevice, CameraDevice>(new ContainerControlledLifetimeManager());
            container.RegisterType<IComponentConfiguration, ComponentConfiguration>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IComponentManager, ComponentManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICameraImageConverter, CameraImageConverter>(new ContainerControlledLifetimeManager());
            container.Resolve<IComponentManager>().Start(args);
            container.Resolve<IComponentConfiguration>().Initialize();
        }

        private static void WaitForExit()
        {
            var run = true;
            while (run)
            {
                var text = Console.ReadLine();
                if (string.IsNullOrEmpty(text))
                    continue;
                if (text.Trim().ToLower().Equals("exit"))
                {
                    run = false;
                }
            }
        }
    }
}