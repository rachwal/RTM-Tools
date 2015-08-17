// RTM.Tools
// RTM.Component.SURFDetector
// Program.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using Emgu.CV.Cuda;
using Microsoft.Practices.Unity;
using RTM.Component.CameraMovementDetector.Detector;
using RTM.Component.CameraMovementDetector.Manager;
using RTM.Converter.CameraImage;
using RTM.Images.Decoder;
using RTM.Images.Decoder.ImageSource;
using RTM.Images.Factory;
using RTM.Images.Factory.Converter;
using BitmapDecoder = RTM.Images.Decoder.Bitmap.BitmapDecoder;
using ImageConverter = RTM.Images.Factory.ImageConverter;

namespace RTM.Component.CameraMovementDetector
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
            container.RegisterType<IImagesDecoder<Bitmap>, BitmapDecoder>();
            container.RegisterType<ICameraImageConverter, CameraImageConverter>();

            container.RegisterType<IDetector, CameraMovementDetector.Detector.Detector>(new ContainerControlledLifetimeManager());
            container.RegisterType<IComponentManager, ComponentManager>(new ContainerControlledLifetimeManager());

            if (CudaInvoke.HasCuda)
            {
                container.RegisterType<IHomography, CudaHomography>(new ContainerControlledLifetimeManager());
            }
            else
            {
                container.RegisterType<IHomography, Homography>(new ContainerControlledLifetimeManager());
            }

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