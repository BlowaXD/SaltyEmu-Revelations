﻿using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Game.Maps;
using SaltyEmu.PathfinderPlugin.Algorithms;

namespace SaltyEmu.PathfinderPlugin
{
    public class PathfinderPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<PathfinderPlugin>();
        public PluginEnableTime EnableTime => PluginEnableTime.PostContainerBuild;
        public string Name => nameof(PathfinderPlugin);

        public void OnLoad()
        {
            Log.Info("Loading...");
            ChickenContainer.Builder.Register(s => new Pathfinder()).As<IPathfinder>();
            Log.Info("Loaded !");
        }

        public void ReloadConfig()
        {
            // no config for pathfinding yet
        }

        public void SaveConfig()
        {
            // no config for pathfinding yet
        }

        public void SaveDefaultConfig()
        {
            // no config for pathfinding yet
        }


        public void OnDisable()
        {
            // nothing to do
        }

        public void OnEnable()
        {
            // maybe we will do that later
        }
    }
}