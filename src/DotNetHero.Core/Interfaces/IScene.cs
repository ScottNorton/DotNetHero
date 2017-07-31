// Authored by Scott B. Norton

namespace DotNetHero.Core.Interfaces
{
    using DotNetHero.Core.Components;

    public interface IScene
    {
        /// <summary>
        /// Load and begin running the scene.
        /// </summary>
        void Load();

        /// <summary>
        /// The scene handles <see cref="MappedInput"/> in it's own way.
        /// </summary>
        /// <param name="input"></param>
        void ProcessInput(MappedInput input);


    }
}