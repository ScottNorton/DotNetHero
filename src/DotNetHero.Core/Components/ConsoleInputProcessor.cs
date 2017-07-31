// Authored by Scott B. Norton

namespace DotNetHero.Core.Components
{
    using System;
    using System.Collections.Generic;

    sealed class ConsoleInputProcessor : SingletonComponent<ConsoleInputProcessor>
    {
        readonly Dictionary<ConsoleKey, MappedInput> keyMap;
        int keyProcessedCount = 0;

        ConsoleInputProcessor()
        {
            this.keyMap = new Dictionary<ConsoleKey, MappedInput> // cba/todo load user mapped keys from file
            {
                { ConsoleKey.Enter, MappedInput.Interact },
                { ConsoleKey.UpArrow, MappedInput.SelectionUp },
                { ConsoleKey.DownArrow, MappedInput.SelectionDown },
                { ConsoleKey.LeftArrow, MappedInput.SelectionLeft },
                { ConsoleKey.RightArrow, MappedInput.SelectionRight },

                { ConsoleKey.W, MappedInput.ShiftN },
                { ConsoleKey.E, MappedInput.ShiftNe },
                { ConsoleKey.D, MappedInput.ShiftE },
                { ConsoleKey.C, MappedInput.ShiftSe },
                { ConsoleKey.X, MappedInput.ShiftS },
                { ConsoleKey.S, MappedInput.ShiftS },
                { ConsoleKey.Z, MappedInput.ShiftSw },
                { ConsoleKey.A, MappedInput.ShiftW },
                { ConsoleKey.Q, MappedInput.ShiftNw }
            };
        }

        internal void Check()
        {
            while (Console.KeyAvailable)
            {
                if (!this.keyMap.TryGetValue(Console.ReadKey(true).Key, out MappedInput input))
                    break;

                SceneManager.Instance.ProcessInput(input);
            }
        }
    }
}