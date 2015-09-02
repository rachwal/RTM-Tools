// RTM.Tools
// RTM.Component.CameraMovementDetector
// CameraMovementDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using OpenRTM.Core;
using RTM.Component.CameraMovementDetector.Drawer;
using RTM.Converter.CameraImage;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public class CameraMovementDetector : ICameraMovementDetector
    {
        private readonly ITransformationDrawer transformationDrawer;
        private readonly ICameraImageConverter converter;
        private readonly IFeaturesDetector featuresDetector;
        private readonly IOpticalFlow opticalFlow;

        private CameraImage prevCameraImage;
        private CameraImage image;

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

        public CameraMovementDetector(ITransformationDrawer drawer, ICameraImageConverter imageConverter,
            IFeaturesDetector detector, IOpticalFlow opticalFlowCalculator)
        {
            opticalFlow = opticalFlowCalculator;
            featuresDetector = detector;
            transformationDrawer = drawer;
            converter = imageConverter;
        }

        public void ProcessImage(CameraImage cameraImage)
        {
            if (prevCameraImage == null || cameraImage.Width != prevCameraImage.Width ||
                cameraImage.Height != prevCameraImage.Height)
            {
                prevCameraImage = cameraImage;
                return;
            }

            var referenceBitmap = converter.ToBitmap(prevCameraImage);
            prevCameraImage = cameraImage.Copy();
            var currentBitmap = converter.ToBitmap(cameraImage);

            var prev = new Image<Gray, byte>(referenceBitmap);
            prev.SmoothGaussian(3, 3, 34.3, 45.3);
            var curr = new Image<Gray, byte>(currentBitmap);
            curr.SmoothGaussian(3, 3, 34.3, 45.3);

            var prevFeatures = featuresDetector.Detect(prev);
            var currFeatures = opticalFlow.Calculate(prev, curr, prevFeatures);

            UpdateImage(currentBitmap, prevFeatures, currFeatures);
        }

        private void UpdateImage(Bitmap bitmap, PointF[][] prevKeyPoints, PointF[] currentKeyPoints)
        {
            var finalBitmap = transformationDrawer.Arrows(bitmap, prevKeyPoints, currentKeyPoints);
            Image = converter.Convert(finalBitmap);
        }
    }
}