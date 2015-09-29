// RTM.Tools
// RTM.Component.StereoImaging
// IParameters.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

namespace RTM.Component.StereoImaging.Configuration
{
    public interface IParameters
    {
        int MinDisparity { get; set; }
        int NumDisparities { get; set; }
        int BlockSize { get; set; }
        int P1 { get; set; }
        int P2 { get; set; }
        int Disp12MaxDiff { get; set; }
        int PreFilterCap { get; set; }
        int UniquenessRatio { get; set; }
        int NumCalibFrames { get; set; }
        int FilterDisparities { get; set; }
        int FilterRadius { get; set; }
        int FilterIterations { get; set; }
    }
}