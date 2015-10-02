// RTM.Tools
// RTM.Component.StereoImaging
// IDisparityBilateralFilterParameters.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

namespace RTM.Component.StereoImaging.Configuration.Parameters
{
    public interface IDisparityBilateralFilterParameters : IParameters
    {
        int FilterRadius { get; set; }
        int FilterIterations { get; set; }
    }
}