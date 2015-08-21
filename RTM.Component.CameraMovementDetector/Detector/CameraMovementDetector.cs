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

namespace RTM.Component.CameraMovementDetector.Detector
{
    public class CameraMovementDetector : ICameraMovementDetector
    {
        private readonly IHomographyCalculator homography;
        private readonly ITransformationDrawer transformation;

        private readonly ICameraImageConverter converter;

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

        public CameraMovementDetector(IHomographyCalculator calculator,
            ITransformationDrawer drawer, ICameraImageConverter imageConverter)
        {
            transformation = drawer;
            homography = calculator;
            converter = imageConverter;
        }

        public void ProcessImage(CameraImage cameraImage)
        {
            if (lastCameraImage == null)
            {
                lastCameraImage = cameraImage;
                return;
            }

            var referenceBitmap = converter.ToBitmap(lastCameraImage);
            lastCameraImage = cameraImage.Copy();
            var currentBitmap = converter.ToBitmap(cameraImage);

            var matrix = homography.Calculate(referenceBitmap, currentBitmap);
            if (matrix == null)
            {
                return;
            }

            UpdateQuadrilateral(matrix);
            UpdateImage(currentBitmap, matrix);
        }

        private void UpdateQuadrilateral(IInputArray matrix)
        {
            var points = CvInvoke.PerspectiveTransform(initialPoints, matrix);
            Quadrilateral = new Quadrilateral
            {
                Point1 = new Point2D {X = points[0].X, Y = points[0].Y},
                Point2 = new Point2D {X = points[1].X, Y = points[1].Y},
                Point3 = new Point2D {X = points[2].X, Y = points[2].Y},
                Point4 = new Point2D {X = points[3].X, Y = points[3].Y}
            };
        }

        private void UpdateImage(Bitmap referenceBitmap, Mat matrix)
        {
            var finalBitmap = transformation.Draw(referenceBitmap, matrix);
            Image = converter.Convert(finalBitmap);
        }
    }
}