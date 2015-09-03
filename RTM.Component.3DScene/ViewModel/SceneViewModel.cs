// RTM.Tools
// RTM.Component.3DScene
// SceneViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Windows;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using OpenRTM.Core;
using RTM.Component._3DScene.DataProvider;

namespace RTM.Component._3DScene.ViewModel
{
    public class SceneViewModel : ISceneViewModel
    {
        private readonly IDataProvider provider;

        public SceneViewModel(IDataProvider imageProvider)
        {
            provider = imageProvider;
            provider.NewVectors += OnNewVectors;
        }

        public Vector3D Vector { get; set; } = new Vector3D();
        public ObservableDataSource<Point> X { get; set; } = new ObservableDataSource<Point>();
        public ObservableDataSource<Point> Y { get; set; } = new ObservableDataSource<Point>();
        public ObservableDataSource<Point> Z { get; set; } = new ObservableDataSource<Point>();

        public ObservableDataSource<Point> Alpha { get; set; } = new ObservableDataSource<Point>();
        public ObservableDataSource<Point> Beta { get; set; } = new ObservableDataSource<Point>();
        public ObservableDataSource<Point> Gamma { get; set; } = new ObservableDataSource<Point>();

        public void Clear()
        {
            X.Collection.Clear();
            Y.Collection.Clear();
            Z.Collection.Clear();
            Alpha.Collection.Clear();
            Beta.Collection.Clear();
            Gamma.Collection.Clear();
        }

        private void OnNewVectors(object sender, EventArgs e)
        {
            var vectors = provider.Vectors;

            X.AppendAsync(Application.Current.Dispatcher, new Point(X.Collection.Count, vectors.Translation.X));
            Y.AppendAsync(Application.Current.Dispatcher, new Point(Y.Collection.Count, vectors.Translation.Y));
            Z.AppendAsync(Application.Current.Dispatcher, new Point(Z.Collection.Count, vectors.Translation.Z));

            Alpha.AppendAsync(Application.Current.Dispatcher,
                new Point(Alpha.Collection.Count, vectors.Rotation.X));
            Beta.AppendAsync(Application.Current.Dispatcher,
                new Point(Beta.Collection.Count, vectors.Rotation.Y));
            Gamma.AppendAsync(Application.Current.Dispatcher,
                new Point(Gamma.Collection.Count, vectors.Rotation.Z));
        }
    }
}