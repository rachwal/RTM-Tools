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
using RTM.Converter.CameraImage;
using RTM.Images.Factory;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public class CameraMovementDetector : ICameraMovementDetector
    {
        private readonly IHomographyCalculator homography;
        private readonly ITranslationCalculator translation;
        private readonly ITransformationDrawer transformationDrawer;

        private readonly ICameraImageConverter cameraConverter;
        private readonly IImageConverter converter;
        private readonly IImageFactory imageFactory;

        private CameraImage image;
        private CameraImage lastCameraImage;
        private Vector3D translationVector;

        public event EventHandler NewTranslationVector;

        public Vector3D TranslationVector
        {
            get { return translationVector; }
            set
            {
                translationVector = value;
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
            ITranslationCalculator translationCalculator, ITransformationDrawer drawer, IImageFactory factory,
            IImageConverter imageConverter,
            ICameraImageConverter cameraImageConverter)
        {
            transformationDrawer = drawer;
            translation = translationCalculator;
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

            UpdateVector(cameraImage, transformation);
            UpdateImage(referenceBitmap, transformation);
        }

        private void UpdateVector(CameraImage cameraImage, Mat transformation)
        {
            var vector = translation.Calculate(transformation, cameraImage.Width, cameraImage.Height);
            TranslationVector = new Vector3D {X = vector[0], Y = vector[1], Z = vector[2]};
        }

        private void UpdateImage(Bitmap referenceBitmap, Mat transformation)
        {
            var finalBitmap = transformationDrawer.Draw(referenceBitmap, transformation);
            var newImage = imageFactory.Create(finalBitmap);
            Image = cameraConverter.Convert(newImage);
        }
    }
}