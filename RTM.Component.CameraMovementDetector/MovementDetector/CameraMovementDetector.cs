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
using RTM.Component.CameraMovementDetector.CornersDetector;
using RTM.Component.CameraMovementDetector.VectorsCalculator;
using RTM.Converter.CameraImage;
using RTM.DTO;

namespace RTM.Component.CameraMovementDetector.MovementDetector
{
    public class CameraMovementDetector : ICameraMovementDetector
    {
        private readonly IComponentConfiguration configuration;
        private readonly ICameraImageConverter converter;
        private readonly ICornersDetector cornersDetector;
        private readonly IVectorsCalculator vectorsCalculator;

        private CameraImage image;
        private Vectors vectors;

        public CameraMovementDetector(ICameraImageConverter imageConverter, ICornersDetector detector,
            IVectorsCalculator calculator, IComponentConfiguration componentConfiguration)
        {
            converter = imageConverter;
            vectorsCalculator = calculator;
            cornersDetector = detector;
            configuration = componentConfiguration;
        }

        public event EventHandler NewImage;

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

        public event EventHandler NewVectors;

        public void ProcessImage(CameraImage cameraImage)
        {
            var bitmap = converter.ToBitmap(cameraImage);

            var corners = cornersDetector.Detect(bitmap);

            if (cornersDetector.MatchFound(corners))
            {
                Vectors = vectorsCalculator.Calculate(corners, bitmap.Size);
                var colorImage = MarkChessboard(bitmap, corners);
                Image = converter.Convert(colorImage.ToBitmap());
            }
            else
            {
                Image = converter.Convert(bitmap);
            }
        }

        private Image<Bgr, byte> MarkChessboard(Bitmap bitmap, VectorOfVectorOfPointF corners)
        {
            var width = configuration.InnerCornersPerChessboardCols;
            var height = configuration.InnerCornersPerChessboardRows;

            var colorImage = new Image<Bgr, byte>(bitmap);
            colorImage.Draw(new LineSegment2DF(corners[0][0], corners[0][width - 1]), new Bgr(Color.Lime), 2);
            colorImage.Draw(new LineSegment2DF(corners[0][width - 1], corners[0][width*height - 1]), new Bgr(Color.Lime),
                2);
            colorImage.Draw(new LineSegment2DF(corners[0][width*height - 1], corners[0][width*(height - 1)]),
                new Bgr(Color.Lime), 2);
            colorImage.Draw(new LineSegment2DF(corners[0][width*(height - 1)], corners[0][0]), new Bgr(Color.Lime), 2);

            return colorImage;
        }
    }
}