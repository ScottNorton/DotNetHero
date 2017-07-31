// Authored by Scott B. Norton

namespace DotNetHero.Core.Components
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///     Based on events and user input, dictates what happens within an active scene.
    /// </summary>
    sealed class SceneManager : SingletonComponent<SceneManager>
    {
        public readonly Dictionary<string, TypeInfo> Scenes;

        public ConsoleRenderer Renderer; // todo create interface for renderer(s)?

        SceneManager()
        {
            this.Scenes = new Dictionary<string, TypeInfo>();
        }

    }
}