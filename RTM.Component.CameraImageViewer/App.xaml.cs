// RTM.Tools
// RTM.Component.CameraImageViewer
// App.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows;

namespace RTM.Component.CameraImageViewer
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new ViewerBootstrapper();
            bootstrapper.Run();
        }
    }
}