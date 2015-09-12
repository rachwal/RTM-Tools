// RTM.Tools
// RTM.Component.CameraMovementDetector
// Program.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Microsoft.Practices.Unity;
using RTM.Calculator.CameraCalibration;
using RTM.Component.CameraMovementDetector.Configuration;
using RTM.Component.CameraMovementDetector.Filter;
using RTM.Component.CameraMovementDetector.Manager;
using RTM.Component.CameraMovementDetector.MovementDetector;
using RTM.Component.CameraMovementDetector.VectorsCalculator;
using RTM.Converter.CameraImage;
using RTM.Detector.ChessboardCorners;

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

            container.RegisterType<ICameraImageConverter, CameraImageConverter>();
            container.RegisterType<IVectorsFilter, VectorFilter>();
            container.RegisterType<IFiltersFactory, FiltersFactory>();
            container.RegisterType<ICameraMovementDetector, MovementDetector.CameraMovementDetector>(new ContainerControlledLifetimeManager());
            container.RegisterType<IChessboardCornersDetector, ChessboardCornersDetector>();
            container.RegisterType<IVectorsCalculator, VectorsCalculator.VectorsCalculator>();
            container.RegisterType<ICameraCalibration, CameraCalibration>();
            container.RegisterType<IComponentConfiguration, ComponentConfiguration>(new ContainerControlledLifetimeManager());
            container.RegisterType<IComponentManager, ComponentManager>(new ContainerControlledLifetimeManager());
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