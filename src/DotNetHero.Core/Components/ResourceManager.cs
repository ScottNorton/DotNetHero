// Authored by Scott B. Norton

namespace DotNetHero.Core.Components
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using DotNetHero.Core.Structures;

    public sealed class ResourceManager : SingletonComponent<ResourceManager>
    {
        const string ResourcePath = "system\\resource\\";

        const string TerrainPath = "terrain\\";
        const string TerrainExtension = "terrain";
        const string TerrainFileHeader = "TerrainFile";

        readonly DirectoryInfo resourceDirectory;
        public readonly Dictionary<uint, GameField> FieldDictionary;

        ResourceManager()
        {
            this.FieldDictionary = new Dictionary<uint, GameField>();

            this.resourceDirectory = new DirectoryInfo(ResourcePath);
            if (this.resourceDirectory.Exists)
                return;

            this.resourceDirectory.Create();
            this.resourceDirectory.CreateSubdirectory(TerrainPath);
        }

        public void LoadAllTerrain()
        {
            foreach (FileInfo file in this.resourceDirectory.GetFiles($"{TerrainPath}*.{TerrainExtension}"))
                this.LoadTerrainToFieldDict(Path.GetFileNameWithoutExtension(file.Name));
        }

        void LoadTerrainToFieldDict(string fileName)
        {
            string path = $"{ResourcePath}{TerrainPath}{fileName}.{TerrainExtension}";

            GameField result;
            using (FileStream fileStream = File.OpenRead(path))
            using (var br = new BinaryReader(fileStream))
            {
                if ((int)fileStream.Length < 8 || br.ReadString() != TerrainFileHeader)
                    throw new FormatException($"Terrian file at {path} is invalid or corrupt.");

                uint id = br.ReadUInt32();
                int width = br.ReadInt32();
                int height = br.ReadInt32();
                result = new GameField(id, fileName, width, height);

                for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    result.FieldNodes[x, y] = new FieldNode(br.ReadBoolean(), (ConsoleColor)br.ReadInt32());
                this.FieldDictionary.Add(id, result);
            }
        }
        void SaveGameFieldTerrain(GameField gameField)
        {
            string path = $"{ResourcePath}{TerrainPath}{gameField.Name}.{TerrainExtension}";

            using (FileStream fileStream = File.OpenWrite(path))
            using (var bw = new BinaryWriter(fileStream))
            {
                bw.Write(TerrainFileHeader);
                bw.Write(BitConverter.GetBytes(gameField.Id));
                bw.Write(BitConverter.GetBytes(gameField.Size.X));
                bw.Write(BitConverter.GetBytes(gameField.Size.Y));

                for (int x = 0; x < gameField.Size.X; x++)
                for (int y = 0; y < gameField.Size.Y; y++)
                {
                    FieldNode node = gameField.FieldNodes[x, y];
                    bw.Write(BitConverter.GetBytes(node.Access));
                    bw.Write(BitConverter.GetBytes((int)node.Color));
                }
            }
        }
    }
}