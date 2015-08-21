// RTM.Tools
// RTM.Component.CameraImageViewer
// ImageProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.Converter.CameraImage;
using RTM.Images.Factory;

namespace RTM.Component.CameraImageViewer.ImageProvider
{
    public class ImageProvider : IImageProvider
    {
        private readonly ICameraImageConverter converter;
        private Image image;

        public event EventHandler NewImage;

        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                NewImage?.Invoke(this, EventArgs.Empty);
            }
        }

        public ImageProvider(ICameraImageConverter cameraImageConverter)
        {
            converter = cameraImageConverter;
        }

        public void SetImage(CameraImage cameraImage)
        {
            Image = converter.ToImage(cameraImage);
        }
    }
}