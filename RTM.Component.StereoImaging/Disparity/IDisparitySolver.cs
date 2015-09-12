// RTM.Tools
// RTM.Component.StereoImaging
// IDisparitySolver.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using Emgu.CV.Structure;

namespace RTM.Component.StereoImaging.Disparity
{
    public interface IDisparitySolver
    {
        void Solve(Image<Gray, byte> left, Image<Gray, byte> right, out Image<Gray, short> disparity,
            out MCvPoint3D32f[] points);
    }
}