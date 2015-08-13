// RTM.Tools
// RTM.Component.HarrisCornerDetector
// HarrisCornerDetector.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Linq;
using OpenRTM.Core;
using OpenRTM.Extension;
using RTM.Component.HarrisCornerDetector.Configuration;
using RTM.Component.HarrisCornerDetector.Detector;
using RTM.Images.Factory;

namespace RTM.Component.HarrisCornerDetector.Component
{
    [Component(Category = "HarrisCornerDetector", Name = "HarrisCornerDetector")]
    [DetailProfile(
        Description = "HarrisCornerDetector Component",
        Language = "C#",
        LanguageType = "Compile",
        MaxInstance = 1,
        Vendor = "AIST",
        Version = "1.0.0")]
    [CustomProfile("CreationDate", "2015/08/11")]
    [CustomProfile("Author", "Bartosz Rachwal")]
    public class HarrisCornerDetector : DataFlowComponent
    {
        [InPort(PortName = "in")] private readonly InPort<CameraImage> inport = new InPort<CameraImage>();

        [OutPort(PortName = "out")] private readonly OutPort<CameraImage> outport = new OutPort<CameraImage>();

        private ConfigurationSet configurationSet;
        private IDetector detector;

        public IDetector Detector
        {
            get { return detector; }
            set
            {
                detector = value;
                if (value != null)
                {
                    Detector.NewImage += OnNewImage;
                }
            }
        }

        private void OnNewImage(object sender, System.EventArgs e)
        {
            outport.Write(Detector.Image);
        }

        public IComponentConfiguration Configuration { get; set; }


        [Configuration(DefaultValue = "1.4", Name = "sigma")]
        public double Sigma
        {
            get { return Configuration?.Sigma ?? 1.4; }
            set
            {
                if (Configuration == null)
                {
                    return;
                }
                Configuration.Sigma = value;
            }
        }

        [Configuration(DefaultValue = "0.04", Name = "k")]
        public float K
        {
            get { return Configuration?.K ?? 0.04f; }
            set
            {
                if (Configuration == null)
                {
                    return;
                }
                Configuration.K = value;
            }
        }

        [Configuration(DefaultValue = "20000", Name = "threshold")]
        public float Threshold
        {
            get { return Configuration?.Threshold ?? 20000; }
            set
            {
                if (Configuration == null)
                {
                    return;
                }
                Configuration.Threshold = value;
            }
        }

        public HarrisCornerDetector()
        {
            Config.OnSetConfigurationSet += OnSetConfigurationSet;
        }

        protected override ReturnCode_t OnActivated(int execHandle)
        {
            inport.OnWrite += OnWrite;
            return base.OnActivated(execHandle);
        }

        private void OnWrite(CameraImage image)
        {
            Detector.Detect(image);
        }

        protected override ReturnCode_t OnDeactivated(int execHandle)
        {
            inport.OnWrite -= OnWrite;
            return base.OnDeactivated(execHandle);
        }

        private void OnSetConfigurationSet(ConfigurationSet obj)
        {
            if (obj == null)
                return;

            if (configurationSet == null)
            {
                configurationSet = obj;
                return;
            }

            if (obj.Id != configurationSet.Id)
                return;

            foreach (var entry in obj.ConfigurationData)
            {
                if (configurationSet.ConfigurationData.ContainsKey(entry.Key))
                {
                    configurationSet.ConfigurationData[entry.Key] = entry.Value;
                }
                else
                {
                    configurationSet.ConfigurationData.Add(entry.Key, entry.Value);
                }
            }

            Config.OnSetConfigurationSet -= OnSetConfigurationSet;

            foreach (var entry in configurationSet.ConfigurationData)
            {
                this.AddConfigurationValue(entry.Key, entry.Value.ToString());
            }

            Config.OnSetConfigurationSet += OnSetConfigurationSet;
        }
    }
}