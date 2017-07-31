﻿// Authored by Scott B. Norton

namespace DotNetHero.Core.Engines
{
    using System;
    using System.Threading;
    using DotNetHero.Core.Components;

    public sealed class ConsoleStandaloneEngine : SingletonComponent<ConsoleStandaloneEngine>, IDisposable
    {
        ConsoleStandaloneEngine()
        {
            this.OnStart += ResourceManager.Instance.LoadAllTerrain;
        }

        public void Dispose()
        {
            // save stuff (settings... or stuff)
        }

        public event Action OnStart;

        public void Start()
        {
            this.OnStart?.Invoke();



            // todo capture and manage main thread for input
            while (true)
                Thread.Sleep(-1);
        }
    }
}