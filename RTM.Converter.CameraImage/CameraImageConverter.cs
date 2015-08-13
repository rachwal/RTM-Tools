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
            return new OpenRTM.Core.CameraImage
            {
                Bpp = (ushort) image.Bpp,
                Pixels = new List<byte>(image.Pixels.ToList().AsReadOnly()),
                Width = (ushort) image.Width,
                Height = (ushort) image.Height,
                Format = image.Format
            };
        }

        public Image Convert(OpenRTM.Core.CameraImage image)
        {
            return new Image
            {
                Bpp = image.Bpp,
                Format = image.Format,
                Width = image.Width,
                Height = image.Height,
                Pixels = image.Pixels.AsReadOnly().ToArray()
            };
        }
    }
}