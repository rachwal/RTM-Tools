// RTM.Tools
// RTM.Component.3DScene
// IDataProvider.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;

namespace RTM.Component._3DScene.DataProvider
{
    public interface IDataProvider
    {
        event EventHandler NewData;
    }
}