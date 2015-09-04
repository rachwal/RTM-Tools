// RTM.Tools
// RTM.Component.CameraMovementDetector
// IFiltersFactory.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;

namespace RTM.Component.CameraMovementDetector.Filter
{
    public interface IFiltersFactory
    {
        KalmanFilter Create();
    }
}