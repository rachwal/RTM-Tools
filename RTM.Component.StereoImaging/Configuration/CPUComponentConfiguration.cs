// RTM.Tools
// RTM.Component.StereoImaging
// CPUComponentConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using RTM.Component.StereoImaging.Configuration.Parameters;

namespace RTM.Component.StereoImaging.Configuration
{
    public class CPUComponentConfiguration : ICPUComponentConfiguration
    {
        private int calibratedFrames;
        private int numCalibFrames;
        private CalibrationStatus calibrationStatus = CalibrationStatus.NotCalibrated;

        private int numDisparities;
        private int minDisparity;
        private int blockSize;
        private int p1;
        private int p2;
        private int disp12MaxDiff;
        private int preFilterCap;
        private int uniquenessRatio;
        private int speckleWindowSize;
        private int speckleRange;

        public event EventHandler ParametersChanged;
        public event EventHandler CalibratedFramesChanged;
        public event EventHandler CalibrationStatusChanged;

        public int MinDisparity
        {
            get { return minDisparity; }
            set
            {
                minDisparity = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int NumDisparities
        {
            get { return numDisparities; }
            set
            {
                numDisparities = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int BlockSize
        {
            get { return blockSize; }
            set
            {
                blockSize = value;
                P1 = 8*BlockSize*BlockSize;
                P2 = 4*P1;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int P1
        {
            get { return p1; }
            set
            {
                p1 = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int P2
        {
            get { return p2; }
            set
            {
                p2 = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int Disp12MaxDiff
        {
            get { return disp12MaxDiff; }
            set
            {
                disp12MaxDiff = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int PreFilterCap
        {
            get { return preFilterCap; }
            set
            {
                preFilterCap = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int UniquenessRatio
        {
            get { return uniquenessRatio; }
            set
            {
                uniquenessRatio = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int SpeckleWindowSize
        {
            get { return speckleWindowSize; }
            set
            {
                speckleWindowSize = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int SpeckleRange
        {
            get { return speckleRange; }
            set
            {
                speckleRange = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int NumCalibFrames
        {
            get { return numCalibFrames; }
            set
            {
                numCalibFrames = value;
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int InnerCornersPerChessboardRows { get; set; }
        public int InnerCornersPerChessboardCols { get; set; }

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
            MinDisparity = 0;
            NumDisparities = 128;
            BlockSize = 11;
            Disp12MaxDiff = -1;
            PreFilterCap = 0;
            UniquenessRatio = 0;
            SpeckleWindowSize = 0;
            SpeckleRange = 1;
            NumCalibFrames = 20;

            InnerCornersPerChessboardCols = 9;
            InnerCornersPerChessboardRows = 6;
        }
    }
}