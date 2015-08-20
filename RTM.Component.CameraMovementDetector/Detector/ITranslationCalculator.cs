// RTM.Tools
// RTM.Component.CameraMovementDetector
// ITranslationCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using Emgu.CV.Util;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public interface ITranslationCalculator
    {
        VectorOfDouble Calculate(Mat transformation, float width, float height);
    }
}