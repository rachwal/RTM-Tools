// RTM.Tools
// RTM.Component.CameraMovementDetector
// VectorFilter.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using OpenRTM.Core;
using RTM.DTO;

namespace RTM.Component.CameraMovementDetector.Filter
{
    public class VectorFilter : IVectorsFilter
    {
        private readonly IFiltersFactory factory;

        private KalmanFilter pitchFilter;
        private KalmanFilter rollFilter;
        private KalmanFilter xFilter;
        private KalmanFilter yawFilter;
        private KalmanFilter yFilter;
        private KalmanFilter zFilter;

        public VectorFilter(IFiltersFactory filtersFactory)
        {
            factory = filtersFactory;
        }

        public Vectors Correct(Matrix<double> rotation, Matrix<double> translation)
        {
            var x = Correct(translation[0, 0], ref xFilter);
            var y = Correct(translation[1, 0], ref yFilter);
            var z = Correct(translation[2, 0], ref zFilter);

            var roll = Correct(rotation[0, 0], ref rollFilter);
            var yaw = Correct(rotation[1, 0], ref yawFilter);
            var pitch = Correct(rotation[2, 0], ref pitchFilter);

            return new Vectors
            {
                Translation = new Vector3D {X = x, Y = y, Z = z},
                Rotation =
                    new Vector3D
                    {
                        X = roll*57.2957795,
                        Y = yaw*57.2957795,
                        Z = pitch*57.2957795
                    }
            };
        }

        private float Correct(double value, ref KalmanFilter filter)
        {
            if (filter == null)
            {
                filter = factory.Create();
            }

            var corrected = new float[2, 1];
            var measurement = new Matrix<float>(1, 1)
            {
                [0, 0] = (float) value
            };

            filter.Correct(measurement.Mat);
            filter.Predict();
            filter.StatePost.CopyTo(corrected);

            return corrected[0, 0];
        }
    }
}