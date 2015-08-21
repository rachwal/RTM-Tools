// RTM.Tools
// RTM.Component.3DScene
// VectorsCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows.Media.Media3D;
using RTM.Component.CameraMovementDetector.Utils;
using RTM.DTO;

namespace RTM.Component._3DScene.Calculator
{
    public class VectorsCalculator : IVectorsCalculator
    {
        private readonly IMathUtils utils;

        public VectorsCalculator(IMathUtils mathUtils)
        {
            utils = mathUtils;
        }

        public Vector3D GetTranslationVector(Quadrilateral quadrilateral)
        {
            var vector = new Vector3D(0, 0, 0);
            return vector;
        }
    }
}