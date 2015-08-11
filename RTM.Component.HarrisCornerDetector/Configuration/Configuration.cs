// RTM.Component.HarrisCornerDetector
// RTM.Component.HarrisCornerDetector
// Configuration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;

namespace RTM.Component.HarrisCornerDetector.Configuration
{
    public interface IComponentConfiguration
    {
        double Sigma { get; set; }
        float K { get; set; }
        float Threshold { get; set; }
        event EventHandler ConfigurationChanged;
    }

    public class ComponentConfiguration : IComponentConfiguration
    {
        private double sigma;
        private float k;
        private float threshold;
        private const double Crit = 0.000001;

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

        public event EventHandler ConfigurationChanged;
    }
}