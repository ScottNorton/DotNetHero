// Authored by Scott B. Norton

namespace DotNetHero.Core.Scenes
{
    using DotNetHero.Core.Components;
    using DotNetHero.Core.Components.Runtime;
    using DotNetHero.Core.Geometry;
    using DotNetHero.Core.Interfaces;
    using DotNetHero.Core.Structures;

    [Scene]
    class World : IScene
    {
        IRenderer renderer;
        Hero hero;
        GameField field;

        public void Load()
        {
            this.renderer = SceneManager.Instance.Renderer;
            this.hero = new Hero();

            var cookie = SceneManager.Instance.Cookie;
            if (cookie is uint)
                this.field = ResourceManager.Instance.FieldDictionary[(uint)cookie];
        }

        public void ProcessInput(MappedInput input)
        {
            switch (input)
            {
                #region [Shift Position]

                case MappedInput.ShiftN:
                    this.hero.Location = this.hero.Location + Xy.North;
                    goto case MappedInput.RefreshMap;
                case MappedInput.ShiftNe:
                    this.hero.Location = this.hero.Location + Xy.Northeast;
                    goto case MappedInput.RefreshMap;
                case MappedInput.ShiftE:
                    this.hero.Location = this.hero.Location + Xy.East;
                    goto case MappedInput.RefreshMap;
                case MappedInput.ShiftSe:
                    this.hero.Location = this.hero.Location + Xy.Southeast;
                    goto case MappedInput.RefreshMap;
                case MappedInput.ShiftS:
                    this.hero.Location = this.hero.Location + Xy.South;
                    goto case MappedInput.RefreshMap;
                case MappedInput.ShiftSw:
                    this.hero.Location = this.hero.Location + Xy.Southwest;
                    goto case MappedInput.RefreshMap;
                case MappedInput.ShiftW:
                    this.hero.Location = this.hero.Location + Xy.West;
                    goto case MappedInput.RefreshMap;
                case MappedInput.ShiftNw:
                    this.hero.Location = this.hero.Location + Xy.Northwest;
                    goto case MappedInput.RefreshMap;

                #endregion [Shift Position]

                case MappedInput.RefreshMap:
                    this.DrawWorld();
                    break;
            }
        }

        void DrawWorld()
        {
            this.renderer.Draw(this.field, this.hero.Location);
        }
    }
}