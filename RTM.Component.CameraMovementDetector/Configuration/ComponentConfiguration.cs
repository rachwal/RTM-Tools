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
        private int innerCornersPerChessboardRows = 9;
        private int innerCornersPerChessboardCols = 6;

        public int InnerCornersPerChessboardRows
        {
            get { return innerCornersPerChessboardRows; }
            set { innerCornersPerChessboardRows = value; }
        }

        public int InnerCornersPerChessboardCols
        {
            get { return innerCornersPerChessboardCols; }
            set { innerCornersPerChessboardCols = value; }
        }
    }
}