// RTM.Tools
// RTM.Component.StereoImaging
// DisparitySolver.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using Emgu.CV.Structure;
using RTM.Component.StereoImaging.Configuration;

namespace RTM.Component.StereoImaging.Disparity
{
    public class DisparitySolver : IDisparitySolver
    {
        private readonly IComponentConfiguration configuration;

        public DisparitySolver(IComponentConfiguration componentConfiguration)
        {
            configuration = componentConfiguration;
        }

        public Image<Gray, byte> Solve(Image<Gray, byte> left, Image<Gray, byte> right)
        {
            using (var stereoSolver = new StereoSGBM(configuration.MinDisparity, configuration.NumDisparities,
                configuration.BlockSize, configuration.P1, configuration.P2, configuration.Disp12MaxDiff,
                configuration.PreFilterCap, configuration.UniquenessRatio))
            {
                var disparity = new Image<Gray, short>(left.Size);
                stereoSolver.Compute(left, right, disparity);
                return disparity.Convert<Gray, byte>();
            }
        }
    }
}