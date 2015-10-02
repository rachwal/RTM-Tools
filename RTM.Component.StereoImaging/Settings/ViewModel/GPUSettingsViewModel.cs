// RTM.Tools
// RTM.Component.StereoImaging
// GPUSettingsViewModel.cs
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
    public class GPUSettingsViewModel : IGPUSettingsViewModel, INotifyPropertyChanged
    {
        private readonly IGPUComponentConfiguration configuration;

        public GPUSettingsViewModel(IGPUComponentConfiguration componentConfiguration)
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

        public int NumDisparities
        {
            get { return configuration.NumDisparities; }
            set
            {
                var val = (value/16)*16;

                configuration.NumDisparities = val;

                if (val*16 != value)
                {
                    OnPropertyChanged(nameof(NumDisparities));
                }

                OnPropertyChanged(nameof(NumDisparitiesLabel));
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

        public int AlgorithmIterations
        {
            get { return configuration.AlgorithmIterations; }
            set
            {
                configuration.AlgorithmIterations = value;
                OnPropertyChanged(nameof(AlgorithmIterationsLabel));
            }
        }

        public int Levels
        {
            get { return configuration.Levels; }
            set
            {
                configuration.Levels = value;
                OnPropertyChanged(nameof(LevelsLabel));
            }
        }

        public int NrPlane
        {
            get { return configuration.NrPlane; }
            set
            {
                configuration.NrPlane = value;
                OnPropertyChanged(nameof(NrPlaneLabel));
            }
        }

        public int FilterRadius
        {
            get { return configuration.FilterRadius; }
            set
            {
                configuration.FilterRadius = value;
                OnPropertyChanged(nameof(FilterRadiusLabel));
            }
        }

        public int FilterIterations
        {
            get { return configuration.FilterIterations; }
            set
            {
                configuration.FilterIterations = value;
                OnPropertyChanged(nameof(FilterIterationsLabel));
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

        public string CalibrationFramesLabel => $"Calibration Frames: {NumCalibFrames}";

        public string NumDisparitiesLabel => $"Num Disparities: {NumDisparities}";
        public string AlgorithmIterationsLabel => $"Algorithm Iterations: {AlgorithmIterations}";
        public string LevelsLabel => $"Levels: {Levels}";
        public string NrPlaneLabel => $"NrPlane: {NrPlane}";

        public string FilterRadiusLabel => $"Filter Radius: {FilterRadius}";
        public string FilterIterationsLabel => $"Filter Iterations: {FilterIterations}";

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand Calibrate
        {
            get
            {
                return new DelegateCommand(() => { configuration.CalibrationStatus = CalibrationStatus.NotCalibrated; });
            }
        }
    }
}