// RTM.Tools
// RTM.Component.StereoImaging
// CPUSettingsView.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using RTM.Component.StereoImaging.Settings.ViewModel;

namespace RTM.Component.StereoImaging.Settings.View
{
    public partial class CPUSettingsView
    {
        public CPUSettingsView(ICPUSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }

        private ICPUSettingsViewModel ViewModel
        {
            get { return (ICPUSettingsViewModel) DataContext; }
            set { DataContext = value; }
        }
    }
}