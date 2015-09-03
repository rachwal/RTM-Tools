// RTM.Tools
// RTM.Component.3DScene
// IDataProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using RTM.DTO;

namespace RTM.Component._3DScene.DataProvider
{
    public interface IDataProvider
    {
        Vectors Vectors { get; set; }
        event EventHandler NewVectors;
    }
}