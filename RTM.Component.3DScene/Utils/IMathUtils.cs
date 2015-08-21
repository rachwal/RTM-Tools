// RTM.Tools
// RTM.Component.3DScene
// IMathUtils.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV.Structure;

namespace RTM.Component.CameraMovementDetector.Utils
{
    public interface IMathUtils
    {
        double Distance(double x1, double y1, double x2, double y2);
        double Perimeter(MCvPoint3D32f[] points);
        double Perimeter(PointF[] points);
    }
}