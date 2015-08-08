// RTM.Tools
// RTM.Component.CameraImageViewer
// CameraPageViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using RTM.Component.CameraImageViewer.Annotations;
using RTM.Component.CameraImageViewer.ImageProvider;
using RTM.Images.Factory;

namespace RTM.Component.CameraImageViewer.ViewModel
{
    public class CameraPageViewModel : ICameraPageViewModel, INotifyPropertyChanged
    {
        private readonly IImageProvider provider;
        private readonly IBitmapSourceFactory factory;

        public ImageSource CameraImage { get; set; }

        public CameraPageViewModel(IImageProvider imageProvider, IBitmapSourceFactory bitmapSourceFactory)
        {
            factory = bitmapSourceFactory;
            provider = imageProvider;
            provider.NewImage += OnNewImage;
        }

        private void OnNewImage(object sender, EventArgs e)
        {
            var image = provider.Image;
            var bitmapSource = factory.Create(image);
            CameraImage = bitmapSource;
            CameraImage.Freeze();
            OnPropertyChanged(nameof(CameraImage));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}