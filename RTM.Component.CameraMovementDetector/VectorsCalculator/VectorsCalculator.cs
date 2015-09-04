// RTM.Tools
// RTM.Component.CameraMovementDetector
// VectorsCalculator.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using RTM.Component.CameraMovementDetector.Configuration;
using RTM.Component.CameraMovementDetector.Filter;
using RTM.DTO;

namespace RTM.Component.CameraMovementDetector.VectorsCalculator
{
    public class VectorsCalculator : IVectorsCalculator
    {
        private readonly IComponentConfiguration configuration;
        private readonly IVectorsFilter vectorsFilter;
        private Matrix<double> cameraMatrix = new Matrix<double>(3, 3);
        private Matrix<double> distortionCoeffs = new Matrix<double>(5, 1);

        private VectorOfVectorOfPoint3D32F model;

        private VectorOfMat rotationVectors = new VectorOfMat();
        private VectorOfMat translationVectors = new VectorOfMat();

        public VectorsCalculator(IComponentConfiguration componentConfiguration, IVectorsFilter filter)
        {
            vectorsFilter = filter;
            configuration = componentConfiguration;
        }

        private int Width => configuration.InnerCornersPerChessboardCols;
        private int Height => configuration.InnerCornersPerChessboardRows;

        public Vectors Calculate(VectorOfVectorOfPointF corners, Size size)
        {
            if (model == null)
            {
                model = GenerateModel(Width, Height);
            }

            CvInvoke.CalibrateCamera(model, corners, size, cameraMatrix, distortionCoeffs,
                rotationVectors, translationVectors, CalibType.Default, new MCvTermCriteria(10));

            var translation = new Matrix<double>(translationVectors[0].Rows, translationVectors[0].Cols,
                translationVectors[0].DataPointer);

            var rotation = new Matrix<double>(rotationVectors[0].Rows, rotationVectors[0].Cols,
                rotationVectors[0].DataPointer);

            var vectors = vectorsFilter.Correct(rotation, translation);

            return vectors;
        }

        private VectorOfVectorOfPoint3D32F GenerateModel(int width, int height)
        {
            var points = new List<MCvPoint3D32f>();
            for (var x = 0; x < height; x++)
            {
                for (var y = 0; y < width; y++)
                {
                    points.Add(new MCvPoint3D32f(x, y, 0));
                }
            }
            return new VectorOfVectorOfPoint3D32F(new[] {points.ToArray()});
        }
    }
}