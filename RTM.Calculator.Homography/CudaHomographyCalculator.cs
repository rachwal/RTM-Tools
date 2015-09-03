// RTM.Tools
// RTM.Calculator.Homography
// CudaHomographyCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;

namespace RTM.Calculator.Homography
{
    // Reference http://www.emgu.com/wiki/index.php/SURF_feature_detector_in_CSharp
    public class CudaHomographyCalculator : IHomographyCalculator
    {
        private int k = 2;
        private float threshold = 300;

        public void Initialize(float hessianThreshold, int nearestNeighbors)
        {
            threshold = hessianThreshold;
            k = nearestNeighbors;
        }

        public Mat Calculate(Bitmap referenceBitmap, Bitmap currentBitmap)
        {
            Mat homography;

            using (var detector = new CudaSURF(threshold))
            using (var model = new Image<Gray, byte>(referenceBitmap))
            using (var observed = new Image<Gray, byte>(currentBitmap))
            using (var modelMat = new GpuMat(model))
            using (var modelKeyPointsRaw = detector.DetectKeyPointsRaw(modelMat))
            using (var modelKeyPoints = new VectorOfKeyPoint())
            using (var modelDescriptorsRaw = detector.ComputeDescriptorsRaw(modelMat, null, modelKeyPointsRaw))
            using (var observedMat = new GpuMat(observed))
            using (var observedKeyPointsRaw = detector.DetectKeyPointsRaw(observedMat))
            using (var observedKeyPoints = new VectorOfKeyPoint())
            using (var observedDescriptorsRaw = detector.ComputeDescriptorsRaw(observedMat, null, observedKeyPointsRaw))
            using (
                var matcher =
                    new CudaBFMatcher(DistanceType.L2))
            using (var matches = new VectorOfVectorOfDMatch())
            {
                matcher.KnnMatch(observedDescriptorsRaw, modelDescriptorsRaw, matches, k);

                detector.DownloadKeypoints(modelKeyPointsRaw, modelKeyPoints);
                detector.DownloadKeypoints(observedKeyPointsRaw, observedKeyPoints);

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
                Features2DToolbox.VoteForUniqueness(matches, threshold, mask);

                var nonZeroCount = CvInvoke.CountNonZero(mask);

                if (nonZeroCount < 4)
                {
                    return null;
                }

                nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
                    matches, mask, 1.5, 20);

                if (nonZeroCount >= 4)
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