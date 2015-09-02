// RTM.Tools
// RTM.DTO
// Vectors.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using OpenRTM.Core;

namespace RTM.DTO
{
    [DataContract]
    public class Vectors
    {
        [DataMember]
        public Vector3D Translation { get; set; }

        [DataMember]
        public Vector3D Rotation { get; set; }
    }
}