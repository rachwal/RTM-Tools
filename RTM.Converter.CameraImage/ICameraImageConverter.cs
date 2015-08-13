// RTM.Tools
// RTM.Converter.CameraImage
// ICameraImageConverter.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using RTM.Images.Factory;

namespace RTM.Converter.CameraImage
{
    public interface ICameraImageConverter
    {
        OpenRTM.Core.CameraImage Convert(Image image);
        Image Convert(OpenRTM.Core.CameraImage image);
    }
}