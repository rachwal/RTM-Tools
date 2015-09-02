// RTM.Tools
// RTM.Component.CameraStabilizer
// ComponentManager.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Threading.Tasks;
using OpenRTM.Extension;
using OpenRTM.IIOP;
using RTM.Component.CameraStabilizer.Component;
using RTM.Component.CameraStabilizer.Stabilizer;

namespace RTM.Component.CameraStabilizer.Manager
{
    public class ComponentManager : IComponentManager
    {
        private readonly ICameraStabilizer cameraStabilizer;

        public ComponentManager(ICameraStabilizer stabilizer)
        {
            cameraStabilizer = stabilizer;
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
                    var comp = manager.CreateComponent<CameraStabilizerComponent>();
                    comp.CameraStabilizer = cameraStabilizer;

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