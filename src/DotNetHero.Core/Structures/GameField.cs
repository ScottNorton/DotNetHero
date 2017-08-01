// Authored by Scott B. Norton

namespace DotNetHero.Core.Structures
{
    using System;
    using DotNetHero.Core.Components;
    using DotNetHero.Core.Geometry;
    using DotNetHero.Core.Interfaces;

    public struct FieldNode
    {
        public FieldNode(bool access, ConsoleColor color)
        {
            this.Color = this.StaticColor = color;
            this.Access = access;
        }

        internal bool Access;

        public ConsoleColor Color;
        public ConsoleColor StaticColor;
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

        public FieldNode this[int x, int y] => this.FieldNodes[x, y];

        public bool UpdateObjectLocation(IFieldObject obj)
        {
            if (!MathEx.Between(obj.Location.X, 0, Size.X) || !MathEx.Between(obj.Location.Y, 0,this.Size.Y))
                return false;

            if (!this[obj.Location.X, obj.Location.Y].Access)
                return false;

            this.FieldNodes[obj.PreviousLocation.X, obj.PreviousLocation.Y].Color = this.FieldNodes[obj.PreviousLocation.X, obj.PreviousLocation.Y].StaticColor;
            this.FieldNodes[obj.Location.X, obj.Location.Y].Color = ConsoleColor.Green;
            return true;
        }
    }
}