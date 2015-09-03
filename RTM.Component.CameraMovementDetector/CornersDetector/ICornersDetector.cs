// RTM.Tools
// RTM.Component.CameraMovementDetector
// ICornersDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Emgu.CV.Util;

namespace RTM.Component.CameraMovementDetector.CornersDetector
{
    public interface ICornersDetector
    {
        VectorOfVectorOfPointF Detect(Bitmap bitmap);
        bool MatchFound(VectorOfVectorOfPointF corners);
    }
}