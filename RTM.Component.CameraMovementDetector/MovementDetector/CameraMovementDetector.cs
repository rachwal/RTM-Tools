// RTM.Tools
// RTM.Component.CameraMovementDetector
// CameraMovementDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OpenRTM.Core;
using RTM.Component.CameraMovementDetector.Configuration;
using RTM.Component.CameraMovementDetector.VectorsCalculator;
using RTM.Converter.CameraImage;
using RTM.Detector.ChessboardCorners;
using RTM.DTO;

namespace RTM.Component.CameraMovementDetector.MovementDetector
{
    public class CameraMovementDetector : ICameraMovementDetector
    {
        private readonly IComponentConfiguration configuration;
        private readonly ICameraImageConverter converter;
        private readonly IChessboardCornersDetector cornersDetector;
        private readonly IVectorsCalculator vectorsCalculator;

        public event EventHandler NewVectors;
        public event EventHandler NewImage;

        private CameraImage image;
        private Vectors vectors;

        public CameraMovementDetector(ICameraImageConverter imageConverter, IChessboardCornersDetector detector,
            IVectorsCalculator calculator, IComponentConfiguration componentConfiguration)
        {
            converter = imageConverter;
            vectorsCalculator = calculator;
            cornersDetector = detector;
            configuration = componentConfiguration;
        }

        public CameraImage Image
        {
            get { return image; }
            set
            {
                image = value;
                NewImage?.Invoke(this, EventArgs.Empty);
            }
        }

        public Vectors Vectors
        {
            get { return vectors; }
            set
            {
                vectors = value;
                NewVectors?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ProcessImage(CameraImage cameraImage)
        {
            var bitmap = converter.ToBitmap(cameraImage);
            var greyImage = new Image<Gray, byte>(bitmap);

            var cornerPoints = cornersDetector.Detect(greyImage, configuration.InnerCornersPerChessboardCols, configuration.InnerCornersPerChessboardRows);

            if (cornerPoints.Size > 1)
            {
                Vectors = vectorsCalculator.Calculate(cornerPoints, greyImage.Size);
                var colorImage = MarkChessboard(bitmap, cornerPoints);
                Image = converter.Convert(colorImage.ToBitmap());
            }
            else
            {
                Image = converter.Convert(bitmap);
            }
        }

        private Image<Bgr, byte> MarkChessboard(Bitmap bitmap, VectorOfPointF corners)
        {
            var width = configuration.InnerCornersPerChessboardCols;
            var height = configuration.InnerCornersPerChessboardRows;

            var colorImage = new Image<Bgr, byte>(bitmap);
            colorImage.Draw(new LineSegment2DF(corners[0], corners[width - 1]), new Bgr(Color.Lime), 2);
            colorImage.Draw(new LineSegment2DF(corners[width - 1], corners[width * height - 1]), new Bgr(Color.Lime),
                2);
            colorImage.Draw(new LineSegment2DF(corners[width * height - 1], corners[width * (height - 1)]),
                new Bgr(Color.Lime), 2);
            colorImage.Draw(new LineSegment2DF(corners[width * (height - 1)], corners[0]), new Bgr(Color.Lime), 2);

            return colorImage;
        }
    }
}