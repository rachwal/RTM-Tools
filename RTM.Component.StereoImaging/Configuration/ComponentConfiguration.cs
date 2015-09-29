// RTM.Tools
// RTM.Component.StereoImaging
// ComponentConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;

namespace RTM.Component.StereoImaging.Configuration
{
    public class ComponentConfiguration : IComponentConfiguration
    {
        public int MinDisparity { get; set; }
        public int NumDisparities { get; set; }

        private int blockSize;

        public int BlockSize
        {
            get { return blockSize; }
            set
            {
                blockSize = value;
                P1 = 8*BlockSize*BlockSize;
                P2 = 4*P1;
            }
        }

        public int P1 { get; set; }
        public int P2 { get; set; }
        public int Disp12MaxDiff { get; set; }
        public int PreFilterCap { get; set; }
        public int UniquenessRatio { get; set; }
        public int SpeckleWindowSize { get; set; }
        public int SpeckleRange { get; set; }
        public int FilterDisparities { get; set; }
        public int FilterRadius { get; set; }
        public int FilterIterations { get; set; }
        public int NumCalibFrames { get; set; }

        public int InnerCornersPerChessboardRows { get; set; }
        public int InnerCornersPerChessboardCols { get; set; }

        private CalibrationStatus calibrationStatus = CalibrationStatus.NotCalibrated;

        public CalibrationStatus CalibrationStatus
        {
            get { return calibrationStatus; }
            set
            {
                calibrationStatus = value;
                CalibrationStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private int calibratedFrames;

        public int CalibratedFrames
        {
            get { return calibratedFrames; }
            set
            {
                calibratedFrames = value;
                CalibratedFramesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler CalibratedFramesChanged;
        public event EventHandler CalibrationStatusChanged;

        public void Initialize()
        {
            MinDisparity = 0;
            NumDisparities = 256;
            BlockSize = 3;
            Disp12MaxDiff = -1;
            PreFilterCap = 0;
            UniquenessRatio = 0;
            SpeckleWindowSize = 0;
            SpeckleRange = 1;
            FilterDisparities = 256;
            FilterRadius = 5;
            FilterIterations = 5;
            NumCalibFrames = 20;

            InnerCornersPerChessboardCols = 9;
            InnerCornersPerChessboardRows = 6;
        }
    }
}