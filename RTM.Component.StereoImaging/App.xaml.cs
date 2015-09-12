// RTM.Tools
// RTM.Component.StereoImaging
// App.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows;

namespace RTM.Component.StereoImaging
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new StereoImagingBootstrapper();
            bootstrapper.Run();
        }
    }
}