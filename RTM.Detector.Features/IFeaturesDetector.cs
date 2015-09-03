// RTM.Tools
// RTM.Detector.Features
// IFeaturesDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace RTM.Detector.Features
{
    public interface IFeaturesDetector
    {
        MKeyPoint[] Detect(IInputArray image, Type detectorType);
    }
}