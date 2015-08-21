// RTM.Tools
// RTM.Component.3DScene
// SceneViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using OpenRTM.Core;
using RTM.Component._3DScene.Annotations;
using RTM.Component._3DScene.Calculator;
using RTM.Component._3DScene.DataProvider;

namespace RTM.Component._3DScene.ViewModel
{
    public class SceneViewModel : ISceneViewModel, INotifyPropertyChanged
    {
        private readonly IDataProvider provider;
        private readonly IVectorsCalculator calculator;
        public Vector3D Vector { get; set; } = new Vector3D();
        public ObservableDataSource<Point> X { get; set; } = new ObservableDataSource<Point>();
        public ObservableDataSource<Point> Y { get; set; } = new ObservableDataSource<Point>();
        public ObservableDataSource<Point> Z { get; set; } = new ObservableDataSource<Point>();

        public SceneViewModel(IDataProvider imageProvider, IVectorsCalculator vectorsCalculator)
        {
            calculator = vectorsCalculator;
            provider = imageProvider;
            provider.NewVector += OnNewVector;
        }

        private void OnNewVector(object sender, EventArgs e)
        {
            var vector = calculator.GetTranslationVector(provider.Quadrilateral);

            X.AppendAsync(Application.Current.Dispatcher, new Point(X.Collection.Count, vector.X));
            Y.AppendAsync(Application.Current.Dispatcher, new Point(Y.Collection.Count, vector.Y));
            Z.AppendAsync(Application.Current.Dispatcher, new Point(Z.Collection.Count, vector.Z));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}