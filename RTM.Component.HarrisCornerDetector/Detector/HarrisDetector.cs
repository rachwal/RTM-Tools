// RTM.Tools
// RTM.Component.HarrisCornerDetector
// HarrisDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Accord.Imaging;
using AForge.Imaging.Filters;
using OpenRTM.Core;
using RTM.Component.HarrisCornerDetector.Configuration;
using RTM.Converter.CameraImage;
using RTM.Images.Factory;
using Image = RTM.Images.Factory.Image;

namespace RTM.Component.HarrisCornerDetector.Detector
{
    public class HarrisDetector : IDetector
    {
        private readonly IComponentConfiguration configuration;
        private readonly ICameraImageConverter cameraConverter;
        private readonly IImageConverter converter;
        private readonly IImageFactory imageFactory;
        
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

        private HarrisCornersDetector harris;
        private CornersMarker corners;
        private int counter;
        private Image averagedImage = new Image { Pixels = new byte[1] };
        private CameraImage image;
        
        public HarrisDetector(IComponentConfiguration componentConfiguration, IImageFactory factory,
            IImageConverter imageConverter, ICameraImageConverter cameraImageConverter)
        {
            cameraConverter = cameraImageConverter;
            converter = imageConverter;
            imageFactory = factory;

            configuration = componentConfiguration;
            configuration.ConfigurationChanged += OnConfigurationChanged;
        }
        
        public void Detect(CameraImage partial)
        {
            var partialImage = cameraConverter.Convert(partial);
            if (counter < 15)
            {
                counter++;
                Average(partialImage);
            }
            else
            {
                Process(partialImage);
            }
        }

        private void Process(Image partial)
        {
            var source = converter.ToBitmap(partial);
            var markedBitmap = corners.Apply(source);
            var resultImage = imageFactory.Create(markedBitmap);

            Image = cameraConverter.Convert(resultImage);

            counter = 0;
            averagedImage = partial;
        }
        
        private void Average(Image partial)
        {
            if (averagedImage.Pixels?.Length != partial.Pixels.Length)
            {
                averagedImage.Pixels = new byte[partial.Pixels.Length];
                counter = 0;
                return;
            }

            for (var i = 0; i < partial.Pixels.Length; i++)
            {
                double current = averagedImage.Pixels[i] + partial.Pixels[i];
                averagedImage.Pixels[i] = Convert.ToByte(current / 2.0);
            }
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            harris = new HarrisCornersDetector(configuration.K)
            {
                Threshold = configuration.Threshold,
                Sigma = configuration.Sigma
            };

            corners = new CornersMarker(harris, Color.White);
        }
    }
}