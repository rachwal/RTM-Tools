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
using OpenRTM.Core;
using RTM.Component.CameraMovementDetector.Configuration;
using RTM.DTO;

namespace RTM.Component.CameraMovementDetector.VectorsCalculator
{
    public class VectorsCalculator : IVectorsCalculator
    {
        private readonly IComponentConfiguration configuration;

        private VectorOfVectorOfPoint3D32F model;

        public VectorsCalculator(IComponentConfiguration componentConfiguration)
        {
            configuration = componentConfiguration;
        }

        private int Width => configuration.InnerCornersPerChessboardCols;
        private int Height => configuration.InnerCornersPerChessboardRows;

        public Vectors Calculate(VectorOfVectorOfPointF corners, Bitmap bitmap)
        {
            if (model == null)
            {
                model = GenerateModel(Width, Height);
            }

            var cameraMatrix = new Matrix<double>(3, 3);
            var distortionCoeffs = new Matrix<double>(5, 1);

            var rotationVectors = new VectorOfMat();
            var translationVectors = new VectorOfMat();

            CvInvoke.CalibrateCamera(model, corners, bitmap.Size, cameraMatrix, distortionCoeffs,
                rotationVectors, translationVectors, CalibType.FixFocalLength, new MCvTermCriteria(10));

            var translation = new Matrix<double>(translationVectors[0].Rows, translationVectors[0].Cols,
                translationVectors[0].DataPointer);
            var rotation = new Matrix<double>(rotationVectors[0].Rows, rotationVectors[0].Cols,
                rotationVectors[0].DataPointer);

            return new Vectors
            {
                Translation = new Vector3D {X = translation[0, 0], Y = translation[1, 0], Z = translation[2, 0]},
                Rotation =
                    new Vector3D
                    {
                        X = rotation[0, 0]*57.2957795,
                        Y = rotation[1, 0]*57.2957795,
                        Z = rotation[2, 0]*57.2957795
                    }
            };
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