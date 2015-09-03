// RTM.Tools
// RTM.Detector.Features
// FeaturesDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.XFeatures2D;

namespace RTM.Detector.Features
{
    public class FeaturesDetector : IFeaturesDetector
    {
        private readonly GFTTDetector gfttDetector = new GFTTDetector();
        private readonly SIFT siftDetector = new SIFT();
        private readonly SURF surfDetector = new SURF(400);

        public MKeyPoint[] Detect(IInputArray image, Type detectorType)
        {
            if (detectorType == typeof (SURF))
            {
                return surfDetector.Detect(image);
            }
            if (detectorType == typeof (SIFT))
            {
                return siftDetector.Detect(image);
            }
            return gfttDetector.Detect(image);
        }
    }
}