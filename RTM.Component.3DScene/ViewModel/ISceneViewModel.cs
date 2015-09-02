// RTM.Tools
// RTM.Component.3DScene
// ISceneViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using OpenRTM.Core;

namespace RTM.Component._3DScene.ViewModel
{
    public interface ISceneViewModel
    {
        Vector3D Vector { get; set; }
        ObservableDataSource<Point> X { get; set; }
        ObservableDataSource<Point> Y { get; set; }
        ObservableDataSource<Point> Z { get; set; }
        ObservableDataSource<Point> Alpha { get; set; }
        ObservableDataSource<Point> Beta { get; set; }
        ObservableDataSource<Point> Gamma { get; set; }
        void Clear();
    }
}