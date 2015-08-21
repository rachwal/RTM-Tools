// RTM.Tools
// RTM.Component.CameraMovementDetector
// CameraMovementDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Emgu.CV;
using OpenRTM.Core;
using RTM.Component.CameraMovementDetector.Drawer;
using RTM.Component.CameraMovementDetector.Homography;
using RTM.Converter.CameraImage;
using RTM.DTO;
using RTM.Images.Factory;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public class CameraMovementDetector : ICameraMovementDetector
    {
        private readonly IHomographyCalculator homography;
        private readonly ITransformationDrawer transformationDrawer;

        private readonly ICameraImageConverter cameraConverter;
        private readonly IImageConverter converter;
        private readonly IImageFactory imageFactory;

        private CameraImage image;
        private CameraImage lastCameraImage;
        private Quadrilateral quadrilateral;

        private readonly PointF[] initialPoints =
        {
            new PointF(-1, -1), new PointF(1, -1), new PointF(1, 1),
            new PointF(-1, 1)
        };

        public event EventHandler NewTranslationVector;

        public Quadrilateral Quadrilateral
        {
            get { return quadrilateral; }
            set
            {
                quadrilateral = value;
                NewTranslationVector?.Invoke(this, EventArgs.Empty);
            }
        }

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

        public CameraMovementDetector(IHomographyCalculator homographyCalculator,
            ITransformationDrawer drawer, IImageFactory factory,
            IImageConverter imageConverter,
            ICameraImageConverter cameraImageConverter)
        {
            transformationDrawer = drawer;
            homography = homographyCalculator;
            cameraConverter = cameraImageConverter;
            converter = imageConverter;
            imageFactory = factory;
        }

        public void ProcessImage(CameraImage cameraImage)
        {
            if (lastCameraImage == null)
            {
                lastCameraImage = cameraImage;
                return;
            }

            var referenceImage = cameraConverter.Convert(lastCameraImage);
            lastCameraImage = cameraImage.Copy();
            var referenceBitmap = converter.ToBitmap(referenceImage);

            var currentImage = cameraConverter.Convert(cameraImage);
            var currentBitmap = converter.ToBitmap(currentImage);

            var transformation = homography.Calculate(referenceBitmap, currentBitmap);
            if (transformation == null)
            {
                return;
            }

            UpdateQuadrilateral(transformation);
            UpdateImage(currentBitmap, transformation);
        }

        private void UpdateQuadrilateral(Mat transformation)
        {
            var points = CvInvoke.PerspectiveTransform(initialPoints, transformation);
            Quadrilateral = new Quadrilateral
            {
                Point1 = new Point2D {X = points[0].X, Y = points[0].Y},
                Point2 = new Point2D {X = points[1].X, Y = points[1].Y},
                Point3 = new Point2D {X = points[2].X, Y = points[2].Y},
                Point4 = new Point2D {X = points[3].X, Y = points[3].Y}
            };
        }

        private void UpdateImage(Bitmap referenceBitmap, Mat transformation)
        {
            var finalBitmap = transformationDrawer.Draw(referenceBitmap, transformation);
            var newImage = imageFactory.Create(finalBitmap);
            Image = cameraConverter.Convert(newImage);
        }
    }
}