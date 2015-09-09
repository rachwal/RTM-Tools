// RTM.Tools
// RTM.Component.StereoImaging
// ComponentManager.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Threading.Tasks;
using OpenRTM.Extension;
using OpenRTM.IIOP;
using RTM.Component.StereoImaging.Component;
using RTM.Component.StereoImaging.Stereo;

namespace RTM.Component.StereoImaging.Manager
{
    public class ComponentManager : IComponentManager
    {
        private readonly IStereoImaging stereoImaging;

        public ComponentManager(IStereoImaging stabilizer)
        {
            stereoImaging = stabilizer;
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
                    var comp = manager.CreateComponent<StereoImagingComponent>();
                    comp.StereoImaging = stereoImaging;

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