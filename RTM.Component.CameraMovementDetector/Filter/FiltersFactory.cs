// RTM.Tools
// RTM.Component.CameraMovementDetector
// FiltersFactory.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using Emgu.CV.Structure;

namespace RTM.Component.CameraMovementDetector.Filter
{
    public class FiltersFactory : IFiltersFactory
    {
        private readonly Matrix<float> errorCovPost = new Matrix<float>(2, 2);
        private readonly Matrix<float> measurementMatrix = new Matrix<float>(new float[,] {{1, 0}});
        private readonly Matrix<float> measurementNoiseCov = new Matrix<float>(1, 1);
        private readonly Matrix<float> processNoiseCov = new Matrix<float>(2, 2);
        private readonly Matrix<float> transitionMatrix = new Matrix<float>(new float[,] {{1, 1}, {0, 1}});

        public KalmanFilter Create()
        {
            var filter = new KalmanFilter(2, 1, 0);
            transitionMatrix.Mat.CopyTo(filter.TransitionMatrix);

            measurementMatrix.SetIdentity();
            measurementMatrix.Mat.CopyTo(filter.MeasurementMatrix);

            processNoiseCov.SetIdentity(new MCvScalar(1.0e-5));
            processNoiseCov.Mat.CopyTo(filter.ProcessNoiseCov);

            measurementNoiseCov.SetIdentity(new MCvScalar(1.0e-2));
            measurementNoiseCov.Mat.CopyTo(filter.MeasurementNoiseCov);

            errorCovPost.SetIdentity();
            errorCovPost.Mat.CopyTo(filter.ErrorCovPost);

            filter.StatePost.SetTo(new float[] {0, 0});

            return filter;
        }
    }
}