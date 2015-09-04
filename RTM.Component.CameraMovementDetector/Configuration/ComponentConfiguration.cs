// RTM.Tools
// RTM.Component.CameraMovementDetector
// ComponentConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

namespace RTM.Component.CameraMovementDetector.Configuration
{
    public class ComponentConfiguration : IComponentConfiguration
    {
        public int InnerCornersPerChessboardRows { get; set; } = 9;
        public int InnerCornersPerChessboardCols { get; set; } = 6;
    }
}