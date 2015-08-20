// RTM.Tools
// RTM.Component.HarrisCornerDetector
// IComponentConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;

namespace RTM.Component.HarrisCornerDetector.Configuration
{
    public interface IComponentConfiguration
    {
        double Sigma { get; set; }
        float K { get; set; }
        float Threshold { get; set; }
        int NoNearestNeighborMatching { get; set; }
        event EventHandler ConfigurationChanged;
    }
}