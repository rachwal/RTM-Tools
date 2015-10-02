// RTM.Tools
// RTM.Component.StereoImaging
// GPUComponentConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using RTM.Component.StereoImaging.Configuration.Parameters;

namespace RTM.Component.StereoImaging.Configuration
{
    public class GPUComponentConfiguration : IGPUComponentConfiguration
    {
        private int calibratedFrames;
        private int numCalibFrames;
        private CalibrationStatus calibrationStatus = CalibrationStatus.NotCalibrated;

        private int filterRadius;
        private int filterIterations;

        private int numDisparities;
        private int algorithmIterations;
        private int levels;
        private int nrPlane;

        public event EventHandler ParametersChanged;
        public event EventHandler CalibratedFramesChanged;
        public event EventHandler CalibrationStatusChanged;

        public int NumDisparities
        {
            get { return numDisparities; }
            set
            {
                numDisparities = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int AlgorithmIterations
        {
            get { return algorithmIterations; }
            set
            {
                algorithmIterations = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int Levels
        {
            get { return levels; }
            set
            {
                levels = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int NrPlane
        {
            get { return nrPlane; }
            set
            {
                nrPlane = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int FilterRadius
        {
            get { return filterRadius; }
            set
            {
                filterRadius = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int FilterIterations
        {
            get { return filterIterations; }
            set
            {
                filterIterations = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int InnerCornersPerChessboardRows { get; set; }
        public int InnerCornersPerChessboardCols { get; set; }

        public int NumCalibFrames
        {
            get { return numCalibFrames; }
            set
            {
                numCalibFrames = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public CalibrationStatus CalibrationStatus
        {
            get { return calibrationStatus; }
            set
            {
                calibrationStatus = value;
                CalibrationStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int CalibratedFrames
        {
            get { return calibratedFrames; }
            set
            {
                calibratedFrames = value;
                CalibratedFramesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Initialize()
        {
            NumDisparities = 128;

            AlgorithmIterations = 8;
            Levels = 4;
            NrPlane = 4;

            FilterRadius = 10;
            FilterIterations = 5;

            NumCalibFrames = 20;

            InnerCornersPerChessboardCols = 9;
            InnerCornersPerChessboardRows = 6;
        }
    }
}