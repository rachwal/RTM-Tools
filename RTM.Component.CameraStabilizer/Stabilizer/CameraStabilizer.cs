// RTM.Tools
// RTM.Component.CameraStabilizer
// CameraStabilizer.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.XFeatures2D;
using OpenRTM.Core;
using RTM.Calculator.OpticalFlow;
using RTM.Converter.CameraImage;
using RTM.Detector.Features;

namespace RTM.Component.CameraStabilizer.Stabilizer
{
    public class CameraStabilizer : ICameraStabilizer
    {
        private readonly ICameraImageConverter converter;
        private readonly IFeaturesDetector featuresDetector;
        private readonly IOpticalFlow opticalFlow;

        private CameraImage referenceImage;
        private CameraImage image;

        public CameraImage Image
        {
            get { return image; }
            set
            {
                image = value;
                NewImage?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler NewImage;

        public CameraStabilizer(IFeaturesDetector features, IOpticalFlow optical, ICameraImageConverter cameraImageConverter)
        {
            opticalFlow = optical;
            featuresDetector = features;
            converter = cameraImageConverter;
        }

        public void ProcessImage(CameraImage cameraImage)
        {
            if (referenceImage == null)
            {
                referenceImage = cameraImage;
                return;
            }

            var referenceBitmap = converter.ToBitmap(referenceImage);
            referenceImage = cameraImage.Copy();
            var currentBitmap = converter.ToBitmap(cameraImage);

            var prev = new Image<Gray, byte>(referenceBitmap);
            var curr = new Image<Gray, byte>(currentBitmap);

            var referenceFeatures = featuresDetector.Detect(prev, typeof(SURF));
            var currentFeatures = opticalFlow.Calculate(prev, curr, referenceFeatures);

            Image = cameraImage;
        }
    }
}