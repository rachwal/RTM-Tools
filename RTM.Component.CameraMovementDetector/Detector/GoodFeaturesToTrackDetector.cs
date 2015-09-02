// RTM.Tools
// RTM.Component.CameraMovementDetector
// GoodFeaturesToTrackDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Features2D;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public class GoodFeaturesToTrackDetector : IFeaturesDetector
    {
        private readonly GFTTDetector detector = new GFTTDetector();

        public PointF[][] Detect(IInputArray image)
        {
            var prevPoints = detector.Detect(image);
            var prevFeatures = new PointF[1][];
            prevFeatures[0] = new PointF[prevPoints.Length];

            for (var i = 0; i < prevPoints.Length; i++)
            {
                prevFeatures[0][i] = prevPoints[i].Point;
            }
            return prevFeatures;
        }
    }
}