// RTM.Tools
// RTM.Component.CameraMovementDetector
// ITransformationDrawer.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV;

namespace RTM.Component.CameraMovementDetector.Drawer
{
    public interface ITransformationDrawer
    {
        Bitmap Transformation(Bitmap referenceBitmap, Mat transformation);
        Bitmap Arrows(Bitmap bitmap, PointF[][] prevKeyPoints, PointF[] currentKeyPoints);
    }
}