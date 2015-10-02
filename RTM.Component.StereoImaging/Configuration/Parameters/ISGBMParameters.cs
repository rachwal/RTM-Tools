// RTM.Tools
// RTM.Component.StereoImaging
// ISGBMParameters.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

namespace RTM.Component.StereoImaging.Configuration.Parameters
{
    public interface ISGBMParameters : IParameters
    {
        int MinDisparity { get; set; }
        int BlockSize { get; set; }
        int P1 { get; set; }
        int P2 { get; set; }
        int Disp12MaxDiff { get; set; }
        int PreFilterCap { get; set; }
        int UniquenessRatio { get; set; }
    }
}