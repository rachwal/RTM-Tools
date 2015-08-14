// RTM.Tools
// RTM.Converter.CameraImage
// CameraImageConverter.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Collections.Generic;
using System.Linq;
using RTM.Images.Factory;

namespace RTM.Converter.CameraImage
{
    public class CameraImageConverter : ICameraImageConverter
    {
        public OpenRTM.Core.CameraImage Convert(Image image)
        {
            var cameraImage = new OpenRTM.Core.CameraImage
            {
                Bpp = (ushort) image.Bpp,
                Pixels = new List<byte>(image.Pixels.ToList().AsReadOnly()),
                Width = (ushort) image.Width,
                Height = (ushort) image.Height,
                Format = image.Format
            };
            image.Pixels = new byte[1];
            return cameraImage;
        }

        public Image Convert(OpenRTM.Core.CameraImage cameraImage)
        {
            var image = new Image(cameraImage.Bpp, cameraImage.Width, cameraImage.Height,
                cameraImage.Pixels.AsReadOnly().ToArray(), cameraImage.Format);
            cameraImage.Pixels = new List<byte>();
            return image;
        }
    }
}