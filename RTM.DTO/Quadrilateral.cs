// RTM.Tools
// RTM.DTO
// Quadrilateral.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using OpenRTM.Core;

namespace RTM.DTO
{
    [DataContract]
    public class Quadrilateral
    {
        [DataMember]
        public Point2D Point1 { get; set; }

        [DataMember]
        public Point2D Point2 { get; set; }

        [DataMember]
        public Point2D Point3 { get; set; }

        [DataMember]
        public Point2D Point4 { get; set; }
    }
}