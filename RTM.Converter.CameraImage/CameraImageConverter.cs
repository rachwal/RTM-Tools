// RTM.Tools
// RTM.Converter.CameraImage
// CameraImageConverter.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RTM.Images.Decoder.ImageSource;
using RTM.Images.Factory;
using Image = RTM.Images.Factory.Image;

namespace RTM.Converter.CameraImage
{
    public class CameraImageConverter : ICameraImageConverter
    {
        private readonly IImageFactory imageFactory = new ImageFactory(new BitmapImageDecoder());
        private readonly IImageConverter imageConverter = new Images.Factory.ImageConverter();

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

        public OpenRTM.Core.CameraImage Convert(Bitmap bitmap)
        {
            var image = imageFactory.Create(bitmap);
            return Convert(image);
        }

        public Image ToImage(OpenRTM.Core.CameraImage cameraImage)
        {
            if (cameraImage == null)
            {
                return Image.Empty;
            }
            var image = new Image(cameraImage.Bpp, cameraImage.Width, cameraImage.Height,
                cameraImage.Pixels.AsReadOnly().ToArray(), cameraImage.Format);
            cameraImage.Pixels = new List<byte>();
            return image;
        }

        public Bitmap ToBitmap(OpenRTM.Core.CameraImage cameraImage)
        {
            var image = ToImage(cameraImage);
            return imageConverter.ToBitmap(image);
        }
    }

    public static class CameraImageExtensions
    {
        public static OpenRTM.Core.CameraImage Copy(this OpenRTM.Core.CameraImage cameraImage)
        {
            return new OpenRTM.Core.CameraImage
            {
                Bpp = cameraImage.Bpp,
                Pixels = new List<byte>(cameraImage.Pixels.AsReadOnly()),
                Width = cameraImage.Width,
                Format = cameraImage.Format,
                Height = cameraImage.Height,
                FDiv = cameraImage.FDiv,
                Time = cameraImage.Time
            };
        }
    }
}