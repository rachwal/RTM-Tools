// RTM.Tools
// RTM.Component.3DScene
// DataProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component._3DScene.DataProvider
{
    public class DataProvider : IDataProvider
    {
        private Vector3D vector;

        public event EventHandler NewVector;

        public Vector3D Vector
        {
            get { return vector; }
            set
            {
                vector = value;
                NewVector?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}