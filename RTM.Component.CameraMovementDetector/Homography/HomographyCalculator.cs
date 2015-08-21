// RTM.Tools
// RTM.Component.CameraMovementDetector
// HomographyCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;

namespace RTM.Component.CameraMovementDetector.Homography
{
    public class HomographyCalculator : IHomographyCalculator
    {
        private const int K = 2;
        private const int HessianThresh = 300;
        private const double UniquenessThreshold = 0.99;

        public Mat Calculate(Bitmap referenceBitmap, Bitmap currentBitmap)
        {
            Mat homography;
            using (var detector = new SURF(HessianThresh))
            using (var model = new Image<Gray, byte>(referenceBitmap))
            using (var modelMat = model.Mat.ToUMat(AccessType.Read))
            using (var modelKeyPoints = new VectorOfKeyPoint())
            using (var modelDescriptors = new UMat())
            using (var observed = new Image<Gray, byte>(currentBitmap))
            using (var observedMat = observed.Mat.ToUMat(AccessType.Read))
            using (var observedKeyPoints = new VectorOfKeyPoint())
            using (var observedDescriptors = new UMat())
            using (var matcher = new BFMatcher(DistanceType.L2))
            using (var matches = new VectorOfVectorOfDMatch())
            {
                detector.DetectAndCompute(modelMat, null, modelKeyPoints, modelDescriptors, false);
                detector.DetectAndCompute(observedMat, null, observedKeyPoints, observedDescriptors, false);

                matcher.Add(modelDescriptors);
                matcher.KnnMatch(observedDescriptors, matches, K, null);

                homography = TryFindHomography(modelKeyPoints, observedKeyPoints, matches);
            }

            return homography;
        }

        private Mat TryFindHomography(VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints,
            VectorOfVectorOfDMatch matches)
        {
            var mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));

            try
            {
                Features2DToolbox.VoteForUniqueness(matches, UniquenessThreshold, mask);

                var nonZeroCount = CvInvoke.CountNonZero(mask);
                if (nonZeroCount < 4)
                {
                    return null;
                }

                nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
                    matches, mask, 1.5, 20);


                if (nonZeroCount > 3)
                {
                    return Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                        observedKeyPoints, matches, mask, 2);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }
    }
}