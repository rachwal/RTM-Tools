// RTM.Tools
// RTM.Component.CameraMovementDetector
// TranslationCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public class TranslationCalculator : ITranslationCalculator
    {
        private readonly Matrix<double> intrinsicMatrix = new Matrix<double>(3, 3)
        {
            [0, 0] = 1.0,
            [0, 1] = 0.0,
            [0, 2] = 0.0,
            [1, 0] = 0.0,
            [1, 1] = 1.0,
            [1, 2] = 0.0,
            [2, 0] = 0.0,
            [2, 1] = 0.0,
            [2, 2] = 1.0
        };

        private readonly Matrix<double> distortionCoeffs = new Matrix<double>(5, 1)
        {
            [0, 0] = 0.0,
            [1, 0] = 0.0,
            [2, 0] = 0.0,
            [3, 0] = 0.0,
            [4, 0] = 0.0
        };

        public VectorOfDouble Calculate(Mat transformation, float width, float height)
        {
            var cameraPoints = new[]
            {
                new PointF(0, 0),
                new PointF(width, 0),
                new PointF(width, height),
                new PointF(0, height),
            };

            var imagePoints = (PointF[]) cameraPoints.Clone();
            imagePoints = CvInvoke.PerspectiveTransform(imagePoints, transformation);

            var rotationVector = new VectorOfDouble();
            var translationVector = new VectorOfDouble();

            var objectPoints = new MCvPoint3D32f[imagePoints.Length];
            for (var i = 0; i < cameraPoints.Length; i++)
            {
                objectPoints[i] = new MCvPoint3D32f(cameraPoints[i].X, cameraPoints[i].Y, 1.0f);
            }

            CvInvoke.SolvePnP(objectPoints, imagePoints, intrinsicMatrix, distortionCoeffs, rotationVector,
                translationVector);

            var scale = Perimeter(objectPoints)/Perimeter(imagePoints);

            if (scale < 1.0)
            {
                return new VectorOfDouble(new[]
                {translationVector[0]/scale, translationVector[1]/scale, translationVector[2]/scale});
            }
            return translationVector;
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
        }

        private double Perimeter(MCvPoint3D32f[] points)
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

        private double Perimeter(PointF[] points)
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