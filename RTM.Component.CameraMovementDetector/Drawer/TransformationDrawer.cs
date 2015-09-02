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

namespace RTM.Component.CameraMovementDetector.Drawer
{
    public class TransformationDrawer : ITransformationDrawer
    {
        public Bitmap Transformation(Bitmap referenceBitmap, Mat transformation)
        {
            var result = new Image<Bgr, byte>(referenceBitmap);
            var rect = new Rectangle(Point.Empty, referenceBitmap.Size);

            float width = referenceBitmap.Width;
            float height = referenceBitmap.Height;

            var side = (float) (Math.Min(width, height)*0.1);
            var offsetX = result.Width*0.5f;
            var offsetY = result.Height*0.5f;

            var currentViewPoints = new[]
            {
                new PointF(offsetX - side, offsetY - side),
                new PointF(offsetX + side, offsetY - side),
                new PointF(offsetX + side, offsetY + side),
                new PointF(offsetX - side, offsetY + side),
                new PointF(offsetX - side, offsetY - side)
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

        public Bitmap Arrows(Bitmap bitmap, PointF[][] prevKeyPoints, PointF[] currentKeyPoints)
        {
            var image = new Image<Bgr, byte>(bitmap);
            for (var i = 0; i < currentKeyPoints.Length; i++)
            {
                var start = prevKeyPoints[0][i];
                var end = currentKeyPoints[i];

                image.Draw(new LineSegment2DF(start, end), new Bgr(Color.Green), 1);
                var startRectangle = new Rectangle((int) start.X - 1, (int) start.Y - 1, 3, 3);
                image.Draw(startRectangle, new Bgr(Color.Blue));
                var endRectangle = new Rectangle((int) end.X - 1, (int) end.Y - 1, 3, 3);
                image.Draw(endRectangle, new Bgr(Color.Red));
            }

            return image.Bitmap;
        }
    }
}