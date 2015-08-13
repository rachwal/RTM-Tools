// RTM.Component.HarrisCornerDetector
// RTM.Component.HarrisCornerDetector
// HarrisDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using System.Linq;
using Accord.Imaging;
using AForge.Imaging.Filters;
using OpenRTM.Core;
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
        private int counter;
        private Image averagedImage = new Image { Pixels = new byte[1] };
        private CameraImage image;

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

        public void Detect(Image partial)
        {
            if (counter < 15)
            {
                counter++;
                Average(partial);
            }
            else
            {
                Process(partial);
            }
        }

        private void Process(Image partial)
        {
            var source = converter.ToBitmap(partial);
            var markedBitmap = corners.Apply(source);
            var resultImage = imageFactory.Create(markedBitmap);

            Image = new CameraImage
            {
                Bpp = (ushort)resultImage.Bpp,
                Width = (ushort)resultImage.Width,
                Height = (ushort)resultImage.Height,
                Format = resultImage.Format,
                Pixels = resultImage.Pixels.ToList()
            };

            counter = 0;
            averagedImage = partial;
        }

        private readonly object o = new object();

        private void Average(Image partial)
        {
            lock (o)
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
    }
}