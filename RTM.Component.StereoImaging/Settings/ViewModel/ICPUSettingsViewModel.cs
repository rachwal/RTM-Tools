// RTM.Tools
// RTM.Component.StereoImaging
// IcpuSettingsViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows.Input;
using RTM.Component.StereoImaging.Configuration.Parameters;

namespace RTM.Component.StereoImaging.Settings.ViewModel
{
    public interface ICPUSettingsViewModel : ISGBMParameters
    {
        ICommand Calibrate { get; }
        string CalibrationFramesLabel { get; }
        string CalibrationStatusLabel { get; }

        string MinDisparityLabel { get; }
        string NumDisparitiesLabel { get; }
        string BlockSizeLabel { get; }
        string P1Label { get; }
        string P2Label { get; }
        string Disp12MaxDiffLabel { get; }
        string PreFilterCapLabel { get; }
        string UniquenessRatioLabel { get; }
    }
}