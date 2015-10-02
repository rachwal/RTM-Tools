// RTM.Tools
// RTM.Component.StereoImaging
// GPUDisparitySolver.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using RTM.Component.StereoImaging.Configuration;

namespace RTM.Component.StereoImaging.Disparity
{
    public class GPUDisparitySolver : IDisparitySolver
    {
        private readonly IGPUComponentConfiguration configuration;

        private CudaStereoConstantSpaceBP algorithm;
        private CudaDisparityBilateralFilter filter;

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

        public GPUDisparitySolver(IGPUComponentConfiguration componentConfiguration)
        {
            configuration = componentConfiguration;
            configuration.ParametersChanged += OnParametersChanged;
        }

        public Image<Gray, byte> Solve(Image<Gray, byte> left, Image<Gray, byte> right)
        {
            var size = left.Size;

            using (var leftGpu = new GpuMat(left.Rows, left.Cols, DepthType.Cv16S, 1))
            using (var rightGpu = new GpuMat(left.Rows, left.Cols, DepthType.Cv16S, 1))
            using (var disparityGpu = new GpuMat(left.Rows, left.Cols, DepthType.Cv16S, 1))
            using (var filteredDisparityGpu = new GpuMat(left.Rows, left.Cols, DepthType.Cv16S, 1))
            using (var filteredDisparity16S = new Mat(size, DepthType.Cv16S, 1))
            using (var filteredDisparity8U = new Mat(size, DepthType.Cv8U, 1))
            {
                leftGpu.Upload(left.Mat);
                rightGpu.Upload(right.Mat);

                algorithm.FindStereoCorrespondence(leftGpu, rightGpu, disparityGpu);

                filter.Apply(disparityGpu, leftGpu, filteredDisparityGpu);

                filteredDisparityGpu.Download(filteredDisparity16S);

                CvInvoke.MinMaxLoc(filteredDisparity16S, ref min, ref max, ref minPosition, ref maxPosition);

                filteredDisparity16S.ConvertTo(filteredDisparity8U, DepthType.Cv8U, 255.0/(Max - Min));

                return new Image<Gray, byte>(filteredDisparity8U.Bitmap);
            }
        }

        private void OnParametersChanged(object sender, System.EventArgs e)
        {
            algorithm = new CudaStereoConstantSpaceBP(configuration.NumDisparities, configuration.AlgorithmIterations,
                configuration.Levels, configuration.NrPlane);
            filter = new CudaDisparityBilateralFilter(configuration.NumDisparities, configuration.FilterRadius,
                configuration.FilterIterations);
        }
    }
}