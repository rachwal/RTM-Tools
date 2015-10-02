// RTM.Tools
// RTM.Component.StereoImaging
// IStereoCalibrationParameters.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;

namespace RTM.Component.StereoImaging.Configuration.Parameters
{
    public enum CalibrationStatus
    {
        NotCalibrated,
        CollectingFrames,
        Calibrating,
        Calibrated
    }

    public interface IStereoCalibrationParameters : IParameters
    {
        int InnerCornersPerChessboardRows { get; set; }
        int InnerCornersPerChessboardCols { get; set; }

        CalibrationStatus CalibrationStatus { get; set; }
        event EventHandler CalibrationStatusChanged;

        int NumCalibFrames { get; set; }
        int CalibratedFrames { get; set; }
        event EventHandler CalibratedFramesChanged;
    }
}