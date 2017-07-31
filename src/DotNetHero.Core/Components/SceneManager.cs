// Authored by Scott B. Norton

namespace DotNetHero.Core.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DotNetHero.Core.Components.Runtime;
    using DotNetHero.Core.Interfaces;

    /// <summary>
    ///     Based on events and user input, dictates what happens within an active scene.
    /// </summary>
    sealed class SceneManager : SingletonComponent<SceneManager>
    {
        public readonly Dictionary<string, TypeInfo> Scenes;
        public object Cookie;

        public IRenderer Renderer { get; private set; }

        public IScene ActiveScene { get; private set; }

        SceneManager()
        {
            this.Scenes = new Dictionary<string, TypeInfo>();
        }

        public void Initialize(IRenderer renderer)
        {
            this.Renderer = renderer;

            IEnumerable<TypeInfo> sceneTypes = typeof(IScene).GetTypeInfo().Assembly.DefinedTypes
                .Where(type => type.GetCustomAttribute<SceneAttribute>() != null);

            foreach (TypeInfo sceneType in sceneTypes)
            {
                if (sceneType.DeclaredConstructors.All(ctor => ctor.GetParameters().Length != 0))
                    throw new Exception($"The {sceneType.Name} scene must have only parameterless constructors or no constructor at all.");

                this.Scenes.Add(sceneType.Name, sceneType);
            }
        }

        public void LoadScene(string name)
        {
            if (!this.Scenes.ContainsKey(name))
                return;

            this.ActiveScene = (IScene)this.Scenes[name].DeclaredConstructors.First().Invoke(null);
            this.ActiveScene.Load();
        }

        /// <summary>
        /// Passes mapped input to the active scene.
        /// </summary>
        public void ProcessInput(MappedInput input)
        {
            this.ActiveScene.ProcessInput(input);
        }
    }
}