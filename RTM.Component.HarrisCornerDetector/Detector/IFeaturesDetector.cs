// RTM.Component.HarrisCornerDetector
// RTM.Component.HarrisCornerDetector
// IFeaturesDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using RTM.Images.Factory;

namespace RTM.Component.HarrisCornerDetector.Detector
{
    public interface IFeaturesDetector
    {
        Image Detect(Image image);
    }
}