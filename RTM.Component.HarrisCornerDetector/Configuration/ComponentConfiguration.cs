// RTM.Tools
// RTM.Component.HarrisCornerDetector
// Configuration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;

namespace RTM.Component.HarrisCornerDetector.Configuration
{
    public class ComponentConfiguration : IComponentConfiguration
    {
        private const double Crit = 0.000001;
        private float k = 0.04f;
        private int noNearestNeighborMatching = 3;
        private double sigma = 1.4;
        private float threshold = 20000;

        public double Sigma
        {
            get { return sigma; }
            set
            {
                if (!(Math.Abs(value - sigma) > Crit))
                {
                    return;
                }
                sigma = value;
                ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public float K
        {
            get { return k; }
            set
            {
                if (!(Math.Abs(value - k) > Crit))
                {
                    return;
                }
                k = value;
                ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public float Threshold
        {
            get { return threshold; }
            set
            {
                if (!(Math.Abs(value - threshold) > Crit))
                {
                    return;
                }
                threshold = value;
                ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int NoNearestNeighborMatching
        {
            get { return noNearestNeighborMatching; }
            set
            {
                if (value == noNearestNeighborMatching)
                {
                    return;
                }
                noNearestNeighborMatching = value;
                ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler ConfigurationChanged;
    }
}