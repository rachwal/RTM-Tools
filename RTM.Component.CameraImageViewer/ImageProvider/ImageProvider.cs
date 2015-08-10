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

        public void SetImage(CameraImage cameraImage)
        {
            Image = new Image
            {
                Bpp = cameraImage.Bpp,
                Format = cameraImage.Format,
                Height = cameraImage.Height,
                Width = cameraImage.Width,
                Pixels = cameraImage.Pixels.ToArray()
            };
        }
    }
}