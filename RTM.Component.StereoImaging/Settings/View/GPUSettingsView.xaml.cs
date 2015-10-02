// RTM.Tools
// RTM.Component.StereoImaging
// GPUSettingsView.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using RTM.Component.StereoImaging.Settings.ViewModel;

namespace RTM.Component.StereoImaging.Settings.View
{
    public partial class GPUSettingsView
    {
        public GPUSettingsView(IGPUSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }

        private IGPUSettingsViewModel ViewModel
        {
            get { return (IGPUSettingsViewModel) DataContext; }
            set { DataContext = value; }
        }
    }
}