// RTM.Tools
// RTM.Component.StereoImaging
// CudaDisparitySolver.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.Structure;
using RTM.Component.StereoImaging.Configuration;

namespace RTM.Component.StereoImaging.Disparity
{
    public class CudaDisparitySolver : IDisparitySolver
    {
        private readonly IComponentConfiguration configuration;

        public CudaDisparitySolver(IComponentConfiguration componentConfiguration)
        {
            configuration = componentConfiguration;
        }

        public Image<Gray, byte> Solve(Image<Gray, byte> left, Image<Gray, byte> right)
        {
            var stereo = new CudaStereoBM(configuration.NumDisparities, configuration.BlockSize);
            var leftGpu = new CudaImage<Gray, byte>(left.Size);
            var rightGpu = new CudaImage<Gray, byte>(right.Size);
            var gpuDisparity = new CudaImage<Gray, byte>(leftGpu.Size);

            var cudaDisparity = new Image<Gray, byte>(gpuDisparity.Size);

            leftGpu.Upload(left.Mat);
            rightGpu.Upload(right.Mat);

            stereo.FindStereoCorrespondence(leftGpu, rightGpu, gpuDisparity);
            gpuDisparity.Download(cudaDisparity);

            return cudaDisparity;
        }
    }
}