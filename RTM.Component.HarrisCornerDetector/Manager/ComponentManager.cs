// RTM.Component.HarrisCornerDetector
// RTM.Component.HarrisCornerDetector
// ComponentManager.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Threading.Tasks;
using OpenRTM.Extension;
using OpenRTM.IIOP;
using RTM.Component.HarrisCornerDetector.Configuration;
using RTM.Component.HarrisCornerDetector.Detector;

namespace RTM.Component.HarrisCornerDetector.Manager
{
    public class ComponentManager : IComponentManager
    {
        private readonly IFeaturesDetector featuresDetector;
        private readonly IComponentConfiguration componentConfiguration;

        public ComponentManager(IFeaturesDetector features, IComponentConfiguration configuration)
        {
            componentConfiguration = configuration;
            featuresDetector = features;
        }

        public void Start(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                var manager = new OpenRTM.Core.Manager(args);
                manager.AddTypes(typeof (CorbaProtocolManager));
                manager.Activate();
                try
                {
                    var comp = manager.CreateComponent<Component.HarrisCornerDetector>();
                    comp.Configuration = componentConfiguration;
                    comp.Detector = featuresDetector;

                    Console.WriteLine(comp.GetComponentProfile().Format());

                    manager.Run();
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("Start naming service before running component");
                    Console.WriteLine(@"More info at http://www.openrtm.org/openrtm/en/node/1420");
                }
            });
        }
    }
}