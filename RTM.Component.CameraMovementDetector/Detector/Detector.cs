// RTM.Tools
// RTM.Component.SURFDetector
// Detector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OpenRTM.Core;
using RTM.Converter.CameraImage;
using RTM.Images.Factory;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public class Detector : IDetector
    {
        private readonly IHomography homography;
        private readonly ICameraImageConverter cameraConverter;
        private readonly IImageConverter converter;
        private readonly IImageFactory imageFactory;
        private CameraImage image;
        private CameraImage lastCameraImage;

        public event EventHandler NewImage;

        public CameraImage Image
        {
            get { return image; }
            set
            {
                image = value;
                NewImage?.Invoke(this, EventArgs.Empty);
            }
        }

        public Detector(IHomography homographyCalculator, IImageFactory factory, IImageConverter imageConverter,
            ICameraImageConverter cameraImageConverter)
        {
            homography = homographyCalculator;
            cameraConverter = cameraImageConverter;
            converter = imageConverter;
            imageFactory = factory;
        }

        public void Detect(CameraImage cameraImage)
        {
            if (lastCameraImage == null)
            {
                lastCameraImage = cameraImage;
                return;
            }

            var referenceImage = cameraConverter.Convert(lastCameraImage);
            lastCameraImage = cameraImage.Copy();
            var currentImage = cameraConverter.Convert(cameraImage);

            var currentBitmap = converter.ToBitmap(currentImage);
            var referenceBitmap = converter.ToBitmap(referenceImage);

            var transformation = homography.Calculate(referenceBitmap, currentBitmap);
            if (transformation == null)
            {
                return;
            }

            var finalBitmap = TransformBitmap(referenceBitmap, transformation);

            var newImage = imageFactory.Create(finalBitmap.Bitmap);
            Image = cameraConverter.Convert(newImage);
        }

        private Mat TransformBitmap(Bitmap referenceBitmap, Mat transformation)
        {
            var result = new Image<Bgr, byte>(referenceBitmap);

            var offsetX = (int) (referenceBitmap.Size.Width/2.5);
            var offsetY = (int) (referenceBitmap.Size.Height/2.5);

            var rect = new Rectangle(Point.Empty, referenceBitmap.Size);
            var pts = new[]
            {
                new PointF(rect.Left + offsetX, rect.Bottom - offsetY),
                new PointF(rect.Right - offsetX, rect.Bottom - offsetY),
                new PointF(rect.Right - offsetX, rect.Bottom - offsetY),
                new PointF(rect.Right - offsetX, rect.Top + offsetY),
                new PointF(rect.Left + offsetX, rect.Top + offsetY)
            };

            var ptsActual = new[]
            {
                new PointF(rect.Left + offsetX, rect.Bottom - offsetY),
                new PointF(rect.Right - offsetX, rect.Bottom - offsetY),
                new PointF(rect.Right - offsetX, rect.Bottom - offsetY),
                new PointF(rect.Right - offsetX, rect.Top + offsetY),
                new PointF(rect.Left + offsetX, rect.Top + offsetY)
            };

            pts = CvInvoke.PerspectiveTransform(pts, transformation);

            var points = Array.ConvertAll(pts, Point.Round);
            using (var vp = new VectorOfPoint(points))
            {
                CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255));
            }

            var pointsActual = Array.ConvertAll(ptsActual, Point.Round);
            using (var vp = new VectorOfPoint(pointsActual))
            {
                CvInvoke.Polylines(result, vp, true, new MCvScalar(0, 255, 255, 0));
            }
            CvInvoke.Line(result, new Point(rect.Size.Width/2, 0), new Point(rect.Size.Width/2, result.Size.Height),
                new MCvScalar(255, 255, 255, 255));
            CvInvoke.Line(result, new Point(0, rect.Size.Height/2), new Point(rect.Size.Width, result.Size.Height/2),
                new MCvScalar(255, 255, 255, 255));
            return result.Mat;
        }
    }
}