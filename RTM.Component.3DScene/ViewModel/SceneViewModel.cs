// RTM.Tools
// RTM.Component.3DScene
// SceneViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.ComponentModel;
using System.Runtime.CompilerServices;
using RTM.Component._3DScene.Annotations;
using RTM.Component._3DScene.DataProvider;

namespace RTM.Component._3DScene.ViewModel
{
    public class SceneViewModel : ISceneViewModel, INotifyPropertyChanged
    {
        private readonly IDataProvider provider;

        public SceneViewModel(IDataProvider imageProvider)
        {
            provider = imageProvider;
            provider.NewData += OnNewData;
        }

        private void OnNewData(object sender, System.EventArgs e)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}