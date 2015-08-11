﻿// RTM.Component.HarrisCornerDetector
// RTM.Component.HarrisCornerDetector
// Program.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using RTM.Component.HarrisCornerDetector.Configuration;
using RTM.Component.HarrisCornerDetector.Detector;
using RTM.Component.HarrisCornerDetector.Manager;
using RTM.Images.Decoder;
using RTM.Images.Decoder.ImageSource;
using RTM.Images.Factory;

namespace RTM.Component.HarrisCornerDetector
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
            container.RegisterType<IBitmapSourceFactory, BitmapSourceFactory>();
            container.RegisterType<IImageFactory, ImageFactory>();
            container.RegisterType<IImagesDecoder<BitmapImage>, BitmapImageDecoder>();
            container.RegisterType<IFeaturesDetector, HarrisDetector>(new ContainerControlledLifetimeManager());
            container.RegisterType<IComponentManager, ComponentManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IComponentConfiguration, ComponentConfiguration>(
                new ContainerControlledLifetimeManager());

            container.Resolve<IComponentManager>().Start(args);
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