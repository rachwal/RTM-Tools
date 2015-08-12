// RTM.Component.HarrisCornerDetector
// RTM.Component.HarrisCornerDetector
// IDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using RTM.Images.Factory;

namespace RTM.Component.HarrisCornerDetector.Detector
{
    public interface IDetector
    {
        Image Detect(Image image);
    }
}