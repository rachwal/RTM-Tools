// RTM.Tools
// RTM.Component.SURFDetector
// ComponentManager.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Threading.Tasks;
using OpenRTM.Extension;
using OpenRTM.IIOP;
using RTM.Component.CameraMovementDetector.Component;
using RTM.Component.CameraMovementDetector.Detector;

namespace RTM.Component.CameraMovementDetector.Manager
{
    public class ComponentManager : IComponentManager
    {
        private readonly IDetector detector;

        public ComponentManager(IDetector features)
        {
            detector = features;
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
                    var comp = manager.CreateComponent<CameraMovementDetectorComponent>();
                    comp.Detector = detector;

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