// RTM.Tools
// RTM.Component.3DScene
// SceneViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.ComponentModel;
using System.Runtime.CompilerServices;
using OpenRTM.Core;
using RTM.Component._3DScene.Annotations;
using RTM.Component._3DScene.DataProvider;

namespace RTM.Component._3DScene.ViewModel
{
    public class SceneViewModel : ISceneViewModel, INotifyPropertyChanged
    {
        private readonly IDataProvider provider;

        public Vector3D Vector { get; set; }

        public SceneViewModel(IDataProvider imageProvider)
        {
            provider = imageProvider;
            provider.NewVector += OnNewVector;
        }

        private void OnNewVector(object sender, System.EventArgs e)
        {
            Vector = provider.Vector;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}