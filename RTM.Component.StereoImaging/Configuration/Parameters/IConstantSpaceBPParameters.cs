// RTM.Tools
// RTM.Component.StereoImaging
// IConstantSpaceBPParameters.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

namespace RTM.Component.StereoImaging.Configuration.Parameters
{
    public interface IConstantSpaceBPParameters : IParameters
    {
        int AlgorithmIterations { get; set; }
        int Levels { get; set; }
        int NrPlane { get; set; }
    }
}