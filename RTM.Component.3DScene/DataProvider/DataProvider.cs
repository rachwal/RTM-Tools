// RTM.Tools
// RTM.Component.3DScene
// DataProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using RTM.DTO;

namespace RTM.Component._3DScene.DataProvider
{
    public class DataProvider : IDataProvider
    {
        private Vectors vectors;

        public event EventHandler NewVectors;

        public Vectors Vectors
        {
            get { return vectors; }
            set
            {
                vectors = value;
                NewVectors?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}