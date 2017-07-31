// Authored by Scott B. Norton

namespace DotNetHero.Core.Geometry
{
    public struct Xy
    {
        public static Xy Empty = new Xy(0, 0);
        public int X, Y;

        public Xy(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static bool operator !=(Xy value, Xy other)
        {
            return !(value == other);
        }

        public static bool operator ==(Xy value, Xy other)
        {
            return value.X == other.X && value.Y == other.Y;
        }

        public static Xy operator -(Xy value, Xy other)
        {
            return new Xy(value.X - other.X, value.Y - other.Y);
        }

        public static Xy operator +(Xy value, Xy other)
        {
            return new Xy(value.X + other.X, value.Y + other.Y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Xy))
                return false;

            var other = (Xy)obj;

            return other.X == this.X &&
                other.Y == this.Y;
        }
    }
}