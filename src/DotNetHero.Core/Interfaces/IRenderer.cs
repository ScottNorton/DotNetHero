namespace DotNetHero.Core.Interfaces
{
    using System;
    using DotNetHero.Core.Geometry;
    using DotNetHero.Core.Structures;

    public interface IRenderer
    {
        event Action OnViewportChange;

        void Draw(GameField field, Xy focalPoint);
    }
}