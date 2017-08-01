
namespace DotNetHero.Core.Structures
{
    using DotNetHero.Core.Geometry;
    using DotNetHero.Core.Interfaces;

    /// <summary>
    /// coneptual class
    /// </summary>
    class Hero : IFieldObject
    {
        Xy location = new Xy(69, 69);

        public Xy Location
        {
            get => this.location;
            set
            {
                this.PreviousLocation = this.location;
                this.location = value;
            }
        }

        public Xy PreviousLocation { get; private set; } = new Xy(70, 70);
    }
}
