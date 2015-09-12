﻿// RTM.Tools
// RTM.Component.StereoImaging
// SettingsView.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using RTM.Component.StereoImaging.Settings.ViewModel;

namespace RTM.Component.StereoImaging.Settings.View
{
    public partial class SettingsView
    {
        public SettingsView(ISettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }

        private ISettingsViewModel ViewModel
        {
            get { return (ISettingsViewModel) DataContext; }
            set { DataContext = value; }
        }
    }
}