// RTM.Component.HarrisCornerDetector
// RTM.Component.HarrisCornerDetector
// HarrisDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Accord.Imaging;
using AForge.Imaging.Filters;
using RTM.Component.HarrisCornerDetector.Configuration;
using RTM.Images.Factory;
using Image = RTM.Images.Factory.Image;

namespace RTM.Component.HarrisCornerDetector.Detector
{
    public class HarrisDetector : IFeaturesDetector
    {
        private readonly IComponentConfiguration configuration;
        private readonly IBitmapSourceFactory bitmapSourceFactory;
        private readonly IImageFactory imageFactory;
        private HarrisCornersDetector harris;
        private CornersMarker corners;

        public HarrisDetector(IComponentConfiguration componentConfiguration, IBitmapSourceFactory sourceFactory,
            IImageFactory factory)
        {
            imageFactory = factory;
            bitmapSourceFactory = sourceFactory;

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
            var source = bitmapSourceFactory.Create(image);

            var markedSource = MarkFeatures(source);

            var resultImage = imageFactory.Create(markedSource);

            return resultImage;
        }

        private BitmapSource MarkFeatures(BitmapSource inImage)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(inImage));
                encoder.Save(outStream);
                bitmap = new Bitmap(outStream);
            }

            var markedBitmap = corners.Apply(bitmap);

            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(markedBitmap.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            return bitmapSource;
        }
    }
}