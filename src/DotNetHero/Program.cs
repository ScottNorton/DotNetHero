// Authored by Scott B. Norton

namespace DotNetHero
{
    using System;
    using System.Linq;
    using System.Threading;
    using DotNetHero.Core.Components;
    using DotNetHero.Core.Geometry;

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = /* I name thee... */"D o t  N e t  H e r o";

            ResourceManager.Instance.LoadAllTerrain();
            var gameField = ResourceManager.Instance.FieldDictionary.Values.First();
            ConsoleRenderer.Instance.Draw(gameField, new Xy(30,30));

            Thread.Sleep(-1);
        }
    }
}