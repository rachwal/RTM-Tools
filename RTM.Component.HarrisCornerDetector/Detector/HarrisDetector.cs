// RTM.Tools
// RTM.Component.HarrisCornerDetector
// HarrisDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Accord.Imaging;
using AForge.Imaging.Filters;
using OpenRTM.Core;
using RTM.Component.HarrisCornerDetector.Configuration;
using RTM.Converter.CameraImage;
using RTM.Images.Factory;

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
        private KNearestNeighborMatching matcher;
        private readonly List<IFeaturePoint> oldPoints = new List<IFeaturePoint>();
        private CameraImage image;

        public HarrisDetector(IComponentConfiguration componentConfiguration, IImageFactory factory,
            IImageConverter imageConverter, ICameraImageConverter cameraImageConverter)
        {
            cameraConverter = cameraImageConverter;
            converter = imageConverter;
            imageFactory = factory;
            configuration = componentConfiguration;

            UpdateDetector();

            configuration.ConfigurationChanged += OnConfigurationChanged;
        }

        public void Detect(CameraImage cameraImage)
        {
            var partial = cameraConverter.Convert(cameraImage);
            var sourceBitmap = converter.ToBitmap(partial);

            var blur = new Blur();
            blur.ApplyInPlace(sourceBitmap);

            var points = harris.ProcessImage(sourceBitmap);
            var featurePoints = points.Select(t => new CornerFeaturePoint(t)).Cast<IFeaturePoint>().ToList();
            if (featurePoints.Count > 0 && oldPoints.Count > 0)
            {
                try
                {
                    var matches = matcher.Match(featurePoints, oldPoints);

                    using (var g = Graphics.FromImage(sourceBitmap))
                    {
                        for (var i = 0; i < matches[0].Length; i++)
                        {
                            g.DrawRectangle(Pens.Blue, matches[0][i].X, matches[0][i].Y, 3, 3);
                            g.DrawRectangle(Pens.Red, matches[1][i].X, matches[1][i].Y, 3, 3);
                            g.DrawLine(Pens.Red, matches[0][i].X + 1, matches[0][i].Y + 1, matches[1][i].X + 1,
                                matches[1][i].Y + 1);
                        }
                    }

                    var resultImage = imageFactory.Create(sourceBitmap);
                    Image = cameraConverter.Convert(resultImage);

                    oldPoints.Clear();
                    oldPoints.AddRange(featurePoints.AsReadOnly());
                }
                catch (Exception)
                {
                }
                finally
                {
                    sourceBitmap.Dispose();
                }
            }
            else
            {
                oldPoints.Clear();
                oldPoints.AddRange(featurePoints.AsReadOnly());
            }
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            UpdateDetector();
        }

        private void UpdateDetector()
        {
            harris = new HarrisCornersDetector(configuration.K)
            {
                Threshold = configuration.Threshold,
                Sigma = configuration.Sigma
            };
            matcher = new KNearestNeighborMatching(configuration.NoNearestNeighborMatching);
        }
    }
}