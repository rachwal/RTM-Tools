// RTM.Tools
// RTM.Component.CameraMovementDetector
// CudaHomographyCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using AForge.Imaging.Filters;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;

namespace RTM.Component.CameraMovementDetector.Homography
{
    public class CudaHomographyCalculator : IHomographyCalculator
    {
        private const int UniquenessThreshold = 300;
        private const int K = 2;

        public Mat Calculate(Bitmap referenceBitmap, Bitmap currentBitmap)
        {
            Mat homography;

            var blur = new Blur();
            blur.ApplyInPlace(referenceBitmap);
            blur.ApplyInPlace(currentBitmap);

            using (var detector = new CudaSURF(UniquenessThreshold))
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
                    new CudaBFMatcher(modelDescriptorsRaw.Depth == DepthType.Cv8U
                        ? DistanceType.Hamming
                        : DistanceType.L2))
            using (var matches = new VectorOfVectorOfDMatch())
            {
                matcher.KnnMatch(observedDescriptorsRaw, modelDescriptorsRaw, matches, K);

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
                Features2DToolbox.VoteForUniqueness(matches, UniquenessThreshold, mask);

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