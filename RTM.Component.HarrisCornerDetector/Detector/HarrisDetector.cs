// RTM.Component.HarrisCornerDetector
// RTM.Component.HarrisCornerDetector
// HarrisDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Accord.Imaging;
using AForge.Imaging.Filters;
using RTM.Component.HarrisCornerDetector.Configuration;
using RTM.Images.Factory;
using Image = RTM.Images.Factory.Image;

namespace RTM.Component.HarrisCornerDetector.Detector
{
    public class HarrisDetector : IDetector
    {
        private readonly IComponentConfiguration configuration;
        private readonly IImageConverter converter;
        private readonly IImageFactory imageFactory;
        private HarrisCornersDetector harris;
        private CornersMarker corners;
        
        public HarrisDetector(IComponentConfiguration componentConfiguration, IImageFactory factory, IImageConverter imageConverter)
        {
            converter = imageConverter;
            imageFactory = factory;
            
            configuration = componentConfiguration;
            configuration.ConfigurationChanged += OnConfigurationChanged;
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

        public Image Detect(Image image)
        {
            var source = converter.ToBitmap(image);

            var markedBitmap = corners.Apply(source);

            var resultImage = imageFactory.Create(markedBitmap);

            return resultImage;
        }
    }
}