// RTM.Tools
// RTM.Component.StereoImaging
// IDisparitySolver.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace RTM.Component.StereoImaging.Disparity
{
    public interface IDisparitySolver
    {
        double Min { get; set; }
        double Max { get; set; }
        Point MinPosition { get; set; }
        Point MaxPosition { get; set; }

        Image<Gray, byte> Solve(Image<Gray, byte> left, Image<Gray, byte> right);
    }
}