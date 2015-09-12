// RTM.Tools
// RTM.Component.3DScene
// SceneBootstrapper.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using RTM.Component._3DScene.DataProvider;
using RTM.Component._3DScene.Manager;
using RTM.Component._3DScene.ViewModel;

namespace RTM.Component._3DScene
{
    public class SceneBootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            Container.RegisterType<IDataProvider, DataProvider.DataProvider>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IComponentManager, ComponentManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISceneViewModel, SceneViewModel>();

            base.ConfigureContainer();
        }

        protected override DependencyObject CreateShell()
        {
            var view = Container.TryResolve<SceneView>();
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