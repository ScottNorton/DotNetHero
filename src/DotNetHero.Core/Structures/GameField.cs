// Authored by Scott B. Norton

namespace DotNetHero.Core.Structures
{
    using System;
    using DotNetHero.Core.Geometry;

    struct FieldNode
    {
        internal ConsoleColor Color;
        internal bool Access;
    }

    public struct GameField
    {
        public readonly string Name;
        public readonly uint Id;

        internal readonly FieldNode[,] FieldNodes;

        public GameField(uint id, string name, int width, int height)
        {
            this.Id = id;
            this.Name = name;
            this.Size = new Xy(width, height);
            this.FieldNodes = new FieldNode[width, height];
        }

        public Xy Size { get; }

        public ConsoleColor this[int x, int y]
        {
            get => this.FieldNodes[x, y].Color;
            set => this.FieldNodes[x, y].Color = value;
        }
    }
}