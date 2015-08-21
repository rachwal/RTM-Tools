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
            get { return (ISceneViewModel)DataContext; }
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
            var x = new LineGraph(ViewModel.X)
            {
                LinePen = new Pen(new SolidColorBrush(Colors.Red), 1),
                Description = new PenDescription("X")
            };
            plotter.Children.Add(x);
            var y = new LineGraph(ViewModel.Y)
            {
                LinePen = new Pen(new SolidColorBrush(Colors.Green), 1),
                Description = new PenDescription("Y")
            };
            plotter.Children.Add(y);
            var z = new LineGraph(ViewModel.Z)
            {
                LinePen = new Pen(new SolidColorBrush(Colors.Blue), 1),
                Description = new PenDescription("Z")
            };
            plotter.Children.Add(z);
        }
    }
}