// RTM.Tools
// RTM.Component.3DScene
// IDataProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using OpenRTM.Core;

namespace RTM.Component._3DScene.DataProvider
{
    public interface IDataProvider
    {
        event EventHandler NewVector;
        Vector3D Vector { get; set; }
    }
}