// RTM.Tools
// RTM.Component.USBCamera
// CameraDevice.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Emgu.CV;
using RTM.Component.USBCamera.Configuration;
using RTM.Component.USBCamera.Data;
using RTM.Converter.CameraImage;

namespace RTM.Component.USBCamera.Device
{
    public class CameraDevice : ICameraDevice
    {
        private readonly IComponentConfiguration configuration;
        private readonly IImageProvider provider;
        private readonly ICameraImageConverter converter;

        private Capture camera;

        public CameraDevice(IComponentConfiguration componentConfiguration, IImageProvider imageProvider,
            ICameraImageConverter cameraImageConverter)
        {
            converter = cameraImageConverter;
            provider = imageProvider;

            configuration = componentConfiguration;
            configuration.CameraIndexChanged += OnCameraIndexChanged;
            configuration.RunningChanged += OnRunningChanged;
        }

        private void OnRunningChanged(object sender, System.EventArgs e)
        {
            if (configuration.Running)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        private void OnCameraIndexChanged(object sender, System.EventArgs e)
        {
            Stop();

            if (camera != null)
            {
                camera.ImageGrabbed -= OnImageGrabbed;
            }

            camera = new Capture(configuration.CameraIndex);
            camera.ImageGrabbed += OnImageGrabbed;

            if (configuration.Running)
            {
                Start();
            }
        }

        public void Start()
        {
            if (camera == null)
            {
                camera = new Capture(configuration.CameraIndex);
                camera.ImageGrabbed += OnImageGrabbed;
            }

            camera.Start();
        }

        private void OnImageGrabbed(object sender, System.EventArgs e)
        {
            var frame = new Mat();
            camera.Retrieve(frame);
            provider.Image = converter.Convert(frame.Bitmap);
        }

        public void Stop()
        {
            camera?.Stop();
        }
    }
}