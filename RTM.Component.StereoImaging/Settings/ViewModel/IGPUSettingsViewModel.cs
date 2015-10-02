// RTM.Tools
// RTM.Component.StereoImaging
// IGPUSettingsViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows.Input;
using RTM.Component.StereoImaging.Configuration.Parameters;

namespace RTM.Component.StereoImaging.Settings.ViewModel
{
    public interface IGPUSettingsViewModel : IConstantSpaceBPParameters, IDisparityBilateralFilterParameters
    {
        ICommand Calibrate { get; }
        string CalibrationFramesLabel { get; }
        string CalibrationStatusLabel { get; }

        string NumDisparitiesLabel { get; }
        string AlgorithmIterationsLabel { get; }
        string LevelsLabel { get; }
        string NrPlaneLabel { get; }

        string FilterRadiusLabel { get; }
        string FilterIterationsLabel { get; }
    }
}