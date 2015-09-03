// RTM.Tools
// RTM.Component.CameraMovementDetector
// IComponentConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

namespace RTM.Component.CameraMovementDetector.Configuration
{
    public interface IComponentConfiguration
    {
        int InnerCornersPerChessboardRows { get; set; }
        int InnerCornersPerChessboardCols { get; set; }
    }
}