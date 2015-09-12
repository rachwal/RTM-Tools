// RTM.Tools
// RTM.Component.CameraMovementDetector
// IVectorsCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV.Util;
using RTM.DTO;

namespace RTM.Component.CameraMovementDetector.VectorsCalculator
{
    public interface IVectorsCalculator
    {
        Vectors Calculate(VectorOfPointF cornerPoints, Size imageSize);
    }
}