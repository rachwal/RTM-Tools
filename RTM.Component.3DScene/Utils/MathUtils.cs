// RTM.Tools
// RTM.Component.3DScene
// MathUtils.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using OpenRTM.Core;
using RTM.DTO;

namespace RTM.Component._3DScene.Utils
{
    public class MathUtils : IMathUtils
    {
        public double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
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

        public double Distance(Point2D point1, Point2D point2)
        {
            return Distance(point1.X, point1.Y, point2.X, point2.Y);
        }

        public double Perimeter(Quadrilateral quadrilateral)
        {
            return Distance(quadrilateral.Point1, quadrilateral.Point2) +
                   Distance(quadrilateral.Point2, quadrilateral.Point3) +
                   Distance(quadrilateral.Point3, quadrilateral.Point4) +
                   Distance(quadrilateral.Point4, quadrilateral.Point1);
        }

        public Point2D Center(Quadrilateral quadrilateral)
        {
            var x = 0.25*
                    (quadrilateral.Point1.X + quadrilateral.Point2.X + quadrilateral.Point3.X + quadrilateral.Point4.X);
            var y = 0.25*
                    (quadrilateral.Point1.Y + quadrilateral.Point2.Y + quadrilateral.Point3.Y + quadrilateral.Point4.Y);
            return new Point2D {X = x, Y = y};
        }
    }
}