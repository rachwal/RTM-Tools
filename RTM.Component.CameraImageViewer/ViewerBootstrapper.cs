// RTM.Tools
// RTM.Component.CameraImageViewer
// ViewerBootstrapper.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using RTM.Component.CameraImageViewer.ImageProvider;
using RTM.Images.Factory;

namespace RTM.Component.CameraImageViewer
{
    public class ViewerBootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            Container.RegisterType<IBitmapSourceFactory, BitmapSourceFactory>();
            Container.RegisterType<IImageProvider, ImageProvider.ImageProvider>(new ContainerControlledLifetimeManager());

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

            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
        }
    }
}