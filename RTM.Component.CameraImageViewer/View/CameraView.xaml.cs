// RTM.Tools
// RTM.Component.CameraImageViewer
// CameraView.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using RTM.Component.CameraImageViewer.ViewModel;

namespace RTM.Component.CameraImageViewer
{
    public partial class CameraView
    {
        private ICameraPageViewModel ViewModel
        {
            get { return (ICameraPageViewModel) DataContext; }
            set { DataContext = value; }
        }

        public CameraView(ICameraPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }
    }
}