// RTM.Tools
// RTM.Component.CameraMovementDetector
// ComponentManager.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Threading.Tasks;
using OpenRTM.Extension;
using OpenRTM.IIOP;
using RTM.Component.CameraMovementDetector.Component;
using RTM.Component.CameraMovementDetector.MovementDetector;

namespace RTM.Component.CameraMovementDetector.Manager
{
    public class ComponentManager : IComponentManager
    {
        private readonly ICameraMovementDetector cameraMovementDetector;

        public ComponentManager(ICameraMovementDetector features)
        {
            cameraMovementDetector = features;
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
                    comp.CameraMovementDetector = cameraMovementDetector;

                    Console.WriteLine(comp.GetComponentProfile().Format());

                    manager.Run();
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine(@"Start naming service before running component");
                    Console.WriteLine(@"More info at http://www.openrtm.org/openrtm/en/node/1420");
                }
            });
        }
    }
}