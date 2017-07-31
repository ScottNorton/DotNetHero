// Authored by Scott B. Norton

namespace DotNetHero
{
    using System;
    using System.IO;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = /* I name thee... */"D o t  N e t  H e r o";

            ConsoleColor[,] terrain;
            int width;
            int height;

            using (FileStream fileStream = File.OpenRead("claycave.terrain"))
            using (var br = new BinaryReader(fileStream))
            {
                string header = br.ReadString();
                uint id = br.ReadUInt32();
                width = br.ReadInt32();
                height = br.ReadInt32();

                terrain = new ConsoleColor[width, height];

                for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    byte access = br.ReadByte();
                    terrain[x, y] = (ConsoleColor)br.ReadInt32();
                }
            }

            int drawX = Console.WindowWidth;
            int drawY = Console.WindowHeight;
            if (width < drawX)
                drawX -= drawX - width;
            if (height < drawY)
                drawY -= drawY - height;

            for (int x = 0; x < drawX; x++)
            for (int y = 0; y < drawY; y++)
            {
                Console.BackgroundColor = terrain[x, y];
                Console.SetCursorPosition(x, y);
                Console.Write(' ');
            }

            Thread.Sleep(-1);
        }
    }
}