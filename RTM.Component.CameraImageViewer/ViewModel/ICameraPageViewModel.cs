// RTM.Tools
// RTM.Component.CameraImageViewer
// ICameraPageViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows.Media;

namespace RTM.Component.CameraImageViewer.ViewModel
{
    public interface ICameraPageViewModel
    {
        ImageSource CameraImage { get; set; }
        string Fps { get; set; }
    }
}