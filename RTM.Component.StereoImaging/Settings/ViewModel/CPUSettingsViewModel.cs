// RTM.Tools
// RTM.Component.StereoImaging
// SettingsViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using RTM.Component.StereoImaging.Configuration;
using RTM.Component.StereoImaging.Configuration.Parameters;

namespace RTM.Component.StereoImaging.Settings.ViewModel
{
    public class CPUSettingsViewModel : ICPUSettingsViewModel, INotifyPropertyChanged
    {
        private readonly ICPUComponentConfiguration configuration;

        public CPUSettingsViewModel(ICPUComponentConfiguration componentConfiguration)
        {
            configuration = componentConfiguration;
            configuration.CalibratedFramesChanged += OnCalibrationStatusFramesChanged;
            configuration.CalibrationStatusChanged += OnCalibrationStatusFramesChanged;
        }

        private void OnCalibrationStatusFramesChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CalibrationStatusLabel));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int MinDisparity
        {
            get { return configuration.MinDisparity; }
            set
            {
                configuration.MinDisparity = value;
                OnPropertyChanged(nameof(MinDisparityLabel));
            }
        }

        public int NumDisparities
        {
            get { return configuration.NumDisparities; }
            set
            {
                var val = (value / 16) * 16;

                configuration.NumDisparities = val;

                if (val * 16 != value)
                {
                    OnPropertyChanged(nameof(NumDisparities));
                }

                OnPropertyChanged(nameof(NumDisparitiesLabel));
            }
        }

        public int BlockSize
        {
            get { return configuration.BlockSize; }
            set
            {
                if (value % 2 == 0)
                {
                    value++;
                }

                configuration.BlockSize = value;
                OnPropertyChanged(nameof(P1Label));
                OnPropertyChanged(nameof(P2Label));
                OnPropertyChanged(nameof(P1));
                OnPropertyChanged(nameof(P2));
                OnPropertyChanged(nameof(BlockSize));
                OnPropertyChanged(nameof(BlockSizeLabel));
            }
        }

        public int P1
        {
            get { return configuration.P1; }
            set
            {
                configuration.P1 = value;
                OnPropertyChanged(nameof(P1Label));
            }
        }

        public int P2
        {
            get { return configuration.P2; }
            set
            {
                configuration.P2 = value;
                OnPropertyChanged(nameof(P2Label));
            }
        }

        public int Disp12MaxDiff
        {
            get { return configuration.Disp12MaxDiff; }
            set
            {
                configuration.Disp12MaxDiff = value;
                OnPropertyChanged(nameof(Disp12MaxDiffLabel));
            }
        }

        public int PreFilterCap
        {
            get { return configuration.PreFilterCap; }
            set
            {
                configuration.PreFilterCap = value;
                OnPropertyChanged(nameof(PreFilterCapLabel));
            }
        }

        public int UniquenessRatio
        {
            get { return configuration.UniquenessRatio; }
            set
            {
                configuration.UniquenessRatio = value;
                OnPropertyChanged(nameof(UniquenessRatioLabel));
            }
        }

        public int NumCalibFrames
        {
            get { return configuration.NumCalibFrames; }
            set
            {
                configuration.NumCalibFrames = value;
                OnPropertyChanged(nameof(CalibrationFramesLabel));
            }
        }
        
        public event EventHandler ParametersChanged;

        public string CalibrationStatusLabel
        {
            get
            {
                switch (configuration.CalibrationStatus)
                {
                    case CalibrationStatus.NotCalibrated:
                        return "Not Calibrated";
                    case CalibrationStatus.Calibrated:
                        return "Calibrated";
                    case CalibrationStatus.CollectingFrames:
                        return $"{configuration.CalibratedFrames} of {configuration.NumCalibFrames} frames ready";
                    case CalibrationStatus.Calibrating:
                        return "Calibrating";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public ICommand Calibrate
        {
            get
            {
                return new DelegateCommand(() => { configuration.CalibrationStatus = CalibrationStatus.NotCalibrated; });
            }
        }

        public string CalibrationFramesLabel => $"Calibration Frames: {NumCalibFrames}";
        public string MinDisparityLabel => $"Min Disparity: {MinDisparity}";
        public string NumDisparitiesLabel => $"Num Disparities: {NumDisparities}";
        public string BlockSizeLabel => $"Block Size: {BlockSize}";
        public string P1Label => $"P1: {P1}";
        public string P2Label => $"P2: {P2}";
        public string Disp12MaxDiffLabel => $"Disp12MaxDiff: {Disp12MaxDiff}";
        public string PreFilterCapLabel => $"PreFilterCap: {PreFilterCap}";
        public string UniquenessRatioLabel => $"UniquenessRatio: {UniquenessRatio}";
      
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}