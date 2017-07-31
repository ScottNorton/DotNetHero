// Authored by Scott B. Norton

namespace DotNetHero
{
    using System;
    using DotNetHero.Core.Engines;

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = /* I name thee... */"D o t  N e t  H e r o";

            using (ConsoleStandaloneEngine.Instance)
                ConsoleStandaloneEngine.Instance.Start();
        }
    }
}