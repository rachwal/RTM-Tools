// RTM.Tools
// RTM.Component.HarrisCornerDetector
// Program.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using RTM.Component.HarrisCornerDetector.Configuration;
using RTM.Component.HarrisCornerDetector.Detector;
using RTM.Component.HarrisCornerDetector.Manager;
using RTM.Converter.CameraImage;
using RTM.Images.Decoder;
using RTM.Images.Decoder.ImageSource;
using RTM.Images.Factory;
using RTM.Images.Factory.Converter;
using ImageConverter = RTM.Images.Factory.ImageConverter;

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

            container.RegisterType<IImageFactory, ImageFactory>();
            container.RegisterType<IImageConverter, ImageConverter>();
            container.RegisterType<IBitmapFactory, BitmapFactory>();
            container.RegisterType<IPixelFormatConverter<PixelFormat>, DrawingPixelFormatConverter>();
            container.RegisterType<IBitmapSourceFactory, BitmapSourceFactory>();
            container.RegisterType<IPixelFormatConverter<System.Windows.Media.PixelFormat?>, MediaPixelFormatConverter>();
            container.RegisterType<IImagesDecoder<BitmapImage>, BitmapImageDecoder>();
            container.RegisterType<IImagesDecoder<Bitmap>, Images.Decoder.Bitmap.BitmapDecoder>();
            container.RegisterType<ICameraImageConverter, CameraImageConverter>();

            container.RegisterType<IHarrisDetector, HarrisDetector>(new ContainerControlledLifetimeManager());
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