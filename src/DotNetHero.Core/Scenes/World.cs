﻿// Authored by Scott B. Norton

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
            this.renderer.OnViewportChange += this.DrawWorld;
            this.hero = new Hero();

            object cookie = SceneManager.Instance.Cookie;
            if (cookie is uint)
                this.field = ResourceManager.Instance.FieldDictionary[(uint)cookie];

            
            this.DrawWorld();
        }

        public void ProcessInput(MappedInput input)
        {
            switch (input)
            {
                #region [Shift Position]

                case MappedInput.ShiftN:
                    this.hero.Location = this.hero.Location + Xy.North;
                    goto case (MappedInput)200;
                case MappedInput.ShiftNe:
                    this.hero.Location = this.hero.Location + Xy.Northeast;
                    goto case (MappedInput)200;
                case MappedInput.ShiftE:
                    this.hero.Location = this.hero.Location + Xy.East;
                    goto case (MappedInput)200;
                case MappedInput.ShiftSe:
                    this.hero.Location = this.hero.Location + Xy.Southeast;
                    goto case (MappedInput)200;
                case MappedInput.ShiftS:
                    this.hero.Location = this.hero.Location + Xy.South;
                    goto case (MappedInput)200;
                case MappedInput.ShiftSw:
                    this.hero.Location = this.hero.Location + Xy.Southwest;
                    goto case (MappedInput)200;
                case MappedInput.ShiftW:
                    this.hero.Location = this.hero.Location + Xy.West;
                    goto case (MappedInput)200;
                case MappedInput.ShiftNw:
                    this.hero.Location = this.hero.Location + Xy.Northwest;
                    goto case (MappedInput)200;

                #endregion [Shift Position]

                //internal cases
                case (MappedInput)200:
                    this.DrawWorld();
                    return;
            }
        }

        void DrawWorld()
        {
            if (!this.field.UpdateObjectLocation(this.hero))
                this.hero.Location = this.hero.PreviousLocation;
            this.renderer.Draw(this.field, this.hero.Location);
        }
    }
}