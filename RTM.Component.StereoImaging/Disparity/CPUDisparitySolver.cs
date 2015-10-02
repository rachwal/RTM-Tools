// RTM.Tools
// RTM.Component.StereoImaging
// CPUDisparitySolver.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using RTM.Component.StereoImaging.Configuration;

namespace RTM.Component.StereoImaging.Disparity
{
    public class CPUDisparitySolver : IDisparitySolver
    {
        private readonly ICPUComponentConfiguration configuration;

        private StereoSGBM algorithm;

        private double min;
        private double max;
        private Point minPosition;
        private Point maxPosition;

        public double Min
        {
            get { return min; }
            set { min = value; }
        }

        public double Max
        {
            get { return max; }
            set { max = value; }
        }

        public Point MinPosition
        {
            get { return minPosition; }
            set { minPosition = value; }
        }

        public Point MaxPosition
        {
            get { return maxPosition; }
            set { maxPosition = value; }
        }

        public CPUDisparitySolver(ICPUComponentConfiguration componentConfiguration)
        {
            configuration = componentConfiguration;
            configuration.ParametersChanged += OnParametersChanged;
        }

        public Image<Gray, byte> Solve(Image<Gray, byte> left, Image<Gray, byte> right)
        {
            using (var disparity16S = new Mat(left.Size, DepthType.Cv16S, 1))
            using (var disparity8U = new Mat(left.Size, DepthType.Cv8U, 1))
            {
                algorithm.Compute(left, right, disparity16S);

                CvInvoke.MinMaxLoc(disparity16S, ref min, ref max, ref minPosition, ref maxPosition);

                disparity16S.ConvertTo(disparity8U, DepthType.Cv8U, 255.0/(Max - Min));

                return new Image<Gray, byte>(disparity8U.Bitmap);
            }
        }

        private void OnParametersChanged(object sender, System.EventArgs e)
        {
            algorithm = new StereoSGBM(configuration.MinDisparity, configuration.NumDisparities,
                configuration.BlockSize, configuration.P1, configuration.P2, configuration.Disp12MaxDiff,
                configuration.PreFilterCap, configuration.UniquenessRatio);
        }
    }
}