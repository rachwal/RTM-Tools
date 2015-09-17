// RTM.Tools
// RTM.Component.StereoImaging
// ICalibration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using Emgu.CV.Structure;

namespace RTM.Component.StereoImaging.CameraCalibration
{
    public interface ICalibration
    {
        void Calibrate(Image<Gray, byte> leftGrayImage, Image<Gray, byte> rightGrayImage);
    }
}