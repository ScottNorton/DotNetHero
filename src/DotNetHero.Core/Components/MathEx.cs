// Authored by Scott B. Norton

namespace DotNetHero.Core.Components
{
    class MathEx
    {
        public static bool Between(int val, int min, int max)
        {
            return val >= min && val <= max;
        }
    }
}