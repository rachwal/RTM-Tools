// RTM.Tools
// RTM.Component.3DScene
// MathUtils.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Emgu.CV.Structure;

namespace RTM.Component.CameraMovementDetector.Utils
{
    public class MathUtils : IMathUtils
    {
        public double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
        }

        public double Perimeter(MCvPoint3D32f[] points)
        {
            var perimeter = 0.0;
            for (var i = 0; i < points.Length - 1; i++)
            {
                perimeter += Distance(points[i].X, points[i].Y, points[i + 1].X,
                    points[i + 1].Y);
            }
            perimeter += Distance(points[0].X, points[0].Y, points[points.Length - 1].X,
                points[points.Length - 1].Y);
            return perimeter;
        }

        public double Perimeter(PointF[] points)
        {
            var perimeter = 0.0;
            for (var i = 0; i < points.Length - 1; i++)
            {
                perimeter += Distance(points[i].X, points[i].Y, points[i + 1].X,
                    points[i + 1].Y);
            }
            perimeter += Distance(points[0].X, points[0].Y, points[points.Length - 1].X,
                points[points.Length - 1].Y);

            return perimeter;
        }
    }
}