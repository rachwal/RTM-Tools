// RTM.Tools
// RTM.Component.3DScene
// DSceneView.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using RTM.Component._3DScene.ViewModel;

namespace RTM.Component._3DScene
{
    public partial class DSceneView
    {
        private ISceneViewModel ViewModel
        {
            get { return (ISceneViewModel) DataContext; }
            set { DataContext = value; }
        }

        public DSceneView(ISceneViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.X.SetXYMapping(p => p);
            plotter.AddLineGraph(ViewModel.X, Colors.Red, 1, "X");

            ViewModel.Y.SetXYMapping(p => p);
            plotter.AddLineGraph(ViewModel.Y, Colors.Green, 1, "Y");

            ViewModel.Z.SetXYMapping(p => p);
            plotter.AddLineGraph(ViewModel.Z, Colors.Blue, 2, "Z");
        }
    }
}