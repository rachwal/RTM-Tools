// RTM.Tools
// RTM.Component.CameraMovementDetector
// IVectorsFilter.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using RTM.DTO;

namespace RTM.Component.CameraMovementDetector.Filter
{
    public interface IVectorsFilter
    {
        Vectors Correct(Matrix<double> rotation, Matrix<double> translation);
    }
}