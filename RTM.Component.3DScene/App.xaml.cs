// RTM.Tools
// RTM.Component.3DScene
// App.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows;

namespace RTM.Component._3DScene
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new SceneBootstrapper();
            bootstrapper.Run();
        }
    }
}