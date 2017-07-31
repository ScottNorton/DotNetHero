// Authored by Scott B. Norton

namespace DotNetHero.Core.Scenes
{
    using DotNetHero.Core.Components;
    using DotNetHero.Core.Components.Runtime;
    using DotNetHero.Core.Interfaces;

    [Scene]
    class Menu : IScene
    {
        public void Load()
        {
            SceneManager.Instance.LoadScene("World");
        }
    }
}