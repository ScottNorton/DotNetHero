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
            SceneManager.Instance.Cookie = (uint)1001;
            //sticking to the one map for now
            // need to come up with a cool way to block input while the renderer is busy..
            SceneManager.Instance.LoadScene("World");
        }

        public void ProcessInput(MappedInput input)
        {
            throw new System.NotImplementedException();
        }
    }
}