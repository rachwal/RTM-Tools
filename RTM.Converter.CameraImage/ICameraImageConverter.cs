// RTM.Tools
// RTM.Converter.CameraImage
// ICameraImageConverter.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Drawing;
using Image = RTM.Images.Factory.Image;

namespace RTM.Converter.CameraImage
{
    public interface ICameraImageConverter
    {
        OpenRTM.Core.CameraImage Convert(Image image);
        OpenRTM.Core.CameraImage Convert(Bitmap bitmap);
        Image ToImage(OpenRTM.Core.CameraImage image);
        Bitmap ToBitmap(OpenRTM.Core.CameraImage cameraImage);
    }
}