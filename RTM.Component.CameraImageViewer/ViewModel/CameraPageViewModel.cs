// RTM.Tools
// RTM.Component.CameraImageViewer
// CameraPageViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

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
        private readonly IImageConverter converter;
        private readonly IImageProvider provider;
        private int frameCounter;

        public CameraPageViewModel(IImageProvider imageProvider, IImageConverter imageConverter)
        {
            converter = imageConverter;
            var timer = new Timer(UpdateFpsLabel);
            timer.Change(0, 1000);

            provider = imageProvider;
            provider.NewImage += OnNewImage;
        }

        public ImageSource CameraImage { get; set; }
        public string Fps { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}