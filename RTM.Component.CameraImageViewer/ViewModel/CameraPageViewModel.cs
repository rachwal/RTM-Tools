// RTM.Tools
// RTM.Component.CameraImageViewer
// CameraPageViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using RTM.Component.CameraImageViewer.Annotations;
using RTM.Component.CameraImageViewer.ImageProvider;
using RTM.Images.Factory;

namespace RTM.Component.CameraImageViewer.ViewModel
{
    public class CameraPageViewModel : ICameraPageViewModel, INotifyPropertyChanged
    {
        private readonly IImageProvider provider;
        private readonly IImageConverter converter;

        private int frameCounter;

        public ImageSource CameraImage { get; set; }
        public string Fps { get; set; }

        public CameraPageViewModel(IImageProvider imageProvider, IImageConverter imageConverter)
        {
            var timer = new Timer(UpdateFpsLabel);
            timer.Change(0, 1000);

            converter = imageConverter;
            provider = imageProvider;
            provider.NewImage += OnNewImage;
        }

        private void UpdateFpsLabel(object state)
        {
            Fps = $"{frameCounter} Fps";
            OnPropertyChanged(nameof(Fps));
            frameCounter = 0;
        }

        private void OnNewImage(object sender, EventArgs e)
        {
            frameCounter++;
            var image = provider.Image;
            var bitmapSource = converter.ToBitmapSource(image);
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