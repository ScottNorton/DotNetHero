// Authored by Scott B. Norton

namespace DotNetHero.Core.Interfaces
{
    using DotNetHero.Core.Geometry;

    public interface IFieldObject
    {
        Xy Location { get; }

        Xy PreviousLocation { get; }
    }
}