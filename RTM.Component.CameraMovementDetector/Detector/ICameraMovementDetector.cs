// RTM.Tools
// RTM.Component.CameraMovementDetector
// ICameraMovementDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component.CameraMovementDetector.Detector
{
    public interface ICameraMovementDetector
    {
        void ProcessImage(CameraImage image);

        event EventHandler NewImage;
        CameraImage Image { get; set; }

        event EventHandler NewTranslationVector;
        Vector3D TranslationVector { get; set; }
    }
}