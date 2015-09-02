// RTM.Tools
// RTM.Component.CameraMovementDetector
// IOpticalFlow.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public interface IOpticalFlow
    {
        PointF[] Calculate(IInputArray prev, IInputArray curr, IReadOnlyList<PointF[]> prevFeatures);
    }
}