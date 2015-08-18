// RTM.Tools
// RTM.Component.SURFDetector
// CudaHomography.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 
//
// SURF Feature Detector Reference 
// http://www.emgu.com/wiki/index.php/SURF_feature_detector_in_CSharp

using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public class CudaHomography : IHomography
    {
        private const int k = 2;
        private const int hessianThresh = 600;
        private const double uniquenessThreshold = 0.9;

        public Mat Calculate(Bitmap referenceBitmap, Bitmap currentBitmap)
        {
            Mat homography = null;

            using (var reference = new Image<Gray, byte>(referenceBitmap))
            using (var current = new Image<Gray, byte>(currentBitmap))
            using (var matches = new VectorOfVectorOfDMatch())
            {
                var modelKeyPoints = new VectorOfKeyPoint();
                var observedKeyPoints = new VectorOfKeyPoint();

                var detector = new CudaSURF(hessianThresh);

                using (var gpuReferenceImage = new GpuMat(reference))
                using (var gpuDetectedKeyPoints = detector.DetectKeyPointsRaw(gpuReferenceImage))
                using (
                    var gpuComputedDescriptors = detector.ComputeDescriptorsRaw(gpuReferenceImage, null,
                        gpuDetectedKeyPoints))
                using (var matcher = new CudaBFMatcher(DistanceType.L2))
                {
                    detector.DownloadKeypoints(gpuDetectedKeyPoints, modelKeyPoints);

                    using (var gpuCurrentImage = new GpuMat(current))
                    using (var detectedKeyPoints = detector.DetectKeyPointsRaw(gpuCurrentImage))
                    using (
                        var computedDescriptors = detector.ComputeDescriptorsRaw(gpuCurrentImage, null,
                            detectedKeyPoints))
                    {
                        matcher.KnnMatch(computedDescriptors, gpuComputedDescriptors, matches, k);
                        detector.DownloadKeypoints(detectedKeyPoints, observedKeyPoints);

                        var mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                        mask.SetTo(new MCvScalar(255));

                        try
                        {
                            Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);
                        }
                        catch (Exception)
                        {
                            return null;
                        }

                        var nonZeroCount = CvInvoke.CountNonZero(mask);

                        if (nonZeroCount < 4)
                        {
                            return null;
                        }

                        nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
                            matches, mask, 1.5, 20);

                        if (nonZeroCount >= 4)
                        {
                            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                                observedKeyPoints, matches, mask, 2);
                        }
                    }
                }
            }

            return homography;
        }
    }
}