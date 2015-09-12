// RTM.Tools
// RTM.Component.StereoImaging
// DisparitySolver.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using Emgu.CV.Structure;
using RTM.Calculator.StereoCalibration;
using RTM.Component.StereoImaging.Configuration;

namespace RTM.Component.StereoImaging.Disparity
{
    public class DisparitySolver : IDisparitySolver
    {
        private readonly IStereoCalibration calibration;
        private readonly IComponentConfiguration configuration;

        public DisparitySolver(IComponentConfiguration componentConfiguration, IStereoCalibration stereoCalibration)
        {
            calibration = stereoCalibration;
            configuration = componentConfiguration;
        }

        public void Solve(Image<Gray, byte> left, Image<Gray, byte> right, out Image<Gray, short> disparity, out MCvPoint3D32f[] points)
        {
            var size = left.Size;

            disparity = new Image<Gray, short>(size);

            using (var stereoSolver = new StereoSGBM(configuration.MinDisparity, configuration.NumDisparities,
                    configuration.BlockSize, configuration.P1, configuration.P2, configuration.Disp12MaxDiff,
                    configuration.PreFilterCap, configuration.UniquenessRatio, configuration.SpeckleWindowSize,
                    configuration.SpeckleRange))
            {
                stereoSolver.Compute(left, right, disparity);
                points = PointCollection.ReprojectImageTo3D(disparity, calibration.DisparityToDepth);
            }
        }
    }
}