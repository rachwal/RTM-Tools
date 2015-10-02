// RTM.Tools
// RTM.Component.StereoImaging
// IParameters.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;

namespace RTM.Component.StereoImaging.Configuration.Parameters
{
    public interface IParameters
    {
        int NumDisparities { get; set; }
        event EventHandler ParametersChanged;
    }
}