﻿// RTM.Tools
// RTM.Component.CameraImageViewer
// ViewerBootstrapper.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using RTM.Component.CameraImageViewer.ImageProvider;
using RTM.Component.CameraImageViewer.Manager;
using RTM.Component.CameraImageViewer.ViewModel;
using RTM.Converter.CameraImage;
using RTM.Images.Factory;

namespace RTM.Component.CameraImageViewer
{
    public class ViewerBootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            Container.RegisterType<IImageConverter, ImageConverter>();
            Container.RegisterType<IImageProvider, ImageProvider.ImageProvider>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IComponentManager, ComponentManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ICameraImageConverter, CameraImageConverter>();
            Container.RegisterType<ICameraPageViewModel, CameraPageViewModel>();

            base.ConfigureContainer();
        }

        protected override DependencyObject CreateShell()
        {
            var view = Container.TryResolve<CameraView>();
            return view;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Container.Resolve<IComponentManager>().Start(null);

            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
        }
    }
}