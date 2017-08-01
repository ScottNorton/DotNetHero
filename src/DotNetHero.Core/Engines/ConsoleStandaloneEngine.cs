// Authored by Scott B. Norton

namespace DotNetHero.Core.Engines
{
    using System;
    using System.Threading;
    using DotNetHero.Core.Components;

    public sealed class ConsoleStandaloneEngine : SingletonComponent<ConsoleStandaloneEngine>, IDisposable
    {
        ConsoleStandaloneEngine()
        {
            this.OnStart += Console.Clear;
            this.OnStart += ResourceManager.Instance.LoadAllTerrain;
            this.OnStart += () => SceneManager.Instance.Initialize(ConsoleRenderer.Instance);
        }

        public void Dispose()
        {
            // save stuff (settings... or stuff)
        }

        public event Action OnStart;

        public void Start()
        {
            this.OnStart?.Invoke();

            SceneManager.Instance.LoadScene("Menu");

            // todo capture and manage main thread for input
            while (true)
            {
                ConsoleInputProcessor.Instance.Check();
                Thread.Sleep(1);
            }
        }
    }
}