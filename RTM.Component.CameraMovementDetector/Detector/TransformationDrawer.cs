// RTM.Tools
// RTM.Component.CameraMovementDetector
// TransformationDrawer.cs
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
    public class TransformationDrawer : ITransformationDrawer
    {
        public Bitmap Draw(Bitmap referenceBitmap, Mat transformation)
        {
            var result = new Image<Bgr, byte>(referenceBitmap);
            var rect = new Rectangle(Point.Empty, referenceBitmap.Size);

            float width = referenceBitmap.Width;
            float height = referenceBitmap.Height;

            var offsetX = (int) (width/2.5);
            var offsetY = (int) (height/2.5);

            var currentViewPoints = new[]
            {
                new PointF(offsetX, height - offsetY),
                new PointF(width - offsetX, height - offsetY),
                new PointF(width - offsetX, height - offsetY),
                new PointF(width - offsetX, offsetY),
                new PointF(offsetX, offsetY)
            };

            var previousViewPoints = (PointF[]) currentViewPoints.Clone();
            previousViewPoints = CvInvoke.PerspectiveTransform(previousViewPoints, transformation);

            var previousPoints = Array.ConvertAll(previousViewPoints, Point.Round);
            using (var vp = new VectorOfPoint(previousPoints))
            {
                CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255));
            }

            var currentPoints = Array.ConvertAll(currentViewPoints, Point.Round);
            using (var vp = new VectorOfPoint(currentPoints))
            {
                CvInvoke.Polylines(result, vp, true, new MCvScalar(0, 255, 255, 0));
            }

            CvInvoke.Line(result, new Point(rect.Size.Width/2, 0), new Point(rect.Size.Width/2, result.Size.Height),
                new MCvScalar(255, 255, 255, 255));
            CvInvoke.Line(result, new Point(0, rect.Size.Height/2), new Point(rect.Size.Width, result.Size.Height/2),
                new MCvScalar(255, 255, 255, 255));
            return result.Bitmap;
        }
    }
}