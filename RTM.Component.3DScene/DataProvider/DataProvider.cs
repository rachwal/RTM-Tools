// RTM.Tools
// RTM.Component.3DScene
// DataProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;
using RTM.DTO;

namespace RTM.Component._3DScene.DataProvider
{
    public class DataProvider : IDataProvider
    {
        private Quadrilateral quadrilateral;

        public event EventHandler NewVector;

        public Quadrilateral Quadrilateral
        {
            get { return quadrilateral; }
            set
            {
                quadrilateral = value;
                NewVector?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}