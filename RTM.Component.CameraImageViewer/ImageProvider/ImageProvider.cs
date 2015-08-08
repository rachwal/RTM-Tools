// RTM.Tools
// RTM.Component.CameraImageViewer
// ImageProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.Images.Factory;

namespace RTM.Component.CameraImageViewer.ImageProvider
{
    public class ImageProvider : IImageProvider
    {
        private IPixelFormatConverter converter;
        private Image image;

        public ImageProvider(IPixelFormatConverter pixelFormatConverter)
        {
            converter = pixelFormatConverter;
        }

        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                NewImage?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler NewImage;

        public void SetImage(CameraImage cameraImage)
        {
        }
    }
}