// Authored by Scott B. Norton

namespace DotNetHero.Core.Components
{
    using System;
    using DotNetHero.Core.Components.Threading;
    using DotNetHero.Core.Geometry;
    using DotNetHero.Core.Interfaces;
    using DotNetHero.Core.Structures;

    public sealed class ConsoleRenderer : ContextThread<ConsoleRenderer>, IRenderer
    {
        const char TransparentDecoration = ' ';

        readonly ContextTimer<ConsoleRenderer> timer;
        ConsoleColor[,] history;
        Xy viewport;

        ConsoleRenderer() => this.timer = new ContextTimer<ConsoleRenderer>(this.CheckViewportState, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));

        public event Action OnViewportChange;

        /// <summary>
        /// Draws a game field to a console window.
        /// </summary>
        /// <remarks>
        /// This will take some extra time to distill before it's complete, will come back to it later.
        /// Need to keep execution of redraws (stide drawing via history) close to 30ms as possible to keep motion smooth.
        /// todo figure out why the buffer is drawn one under size in both axises
        /// </remarks>
        public void Draw(GameField field, Xy focusPoint)
        {
            if (!MathEx.Between(focusPoint.X, 0, field.Size.X) || !MathEx.Between(focusPoint.Y, 0, field.Size.Y))
                return;

            if (!this.InThreadContext)
            {
                this.PostAsync(() => this.Draw(field, focusPoint));
                return;
            }

            this.CheckViewportState();

            var drawModelFrom = new Xy(Math.Max(focusPoint.X - this.viewport.Y / 2, 0), Math.Max(focusPoint.Y - this.viewport.Y / 2, 0));

            var drawSize = new Xy(
                (int)(field.Size.X * (1f / field.Size.X * this.viewport.X)),
                (int)(field.Size.Y * (1f / field.Size.Y * this.viewport.Y)));

            if (drawModelFrom.X + drawSize.X > field.Size.X)
                drawModelFrom.X = Math.Max(drawModelFrom.X - (drawModelFrom.X + drawSize.X - field.Size.X), 0);
            if (drawModelFrom.Y + drawSize.Y > field.Size.Y)
                drawModelFrom.Y = Math.Max(drawModelFrom.Y - (drawModelFrom.Y + drawSize.Y - field.Size.Y), 0);

            Xy fieldDelta = field.Size - this.viewport;
            Xy renderOffset = Xy.Empty;

            if (fieldDelta.X < 0)
            {
                drawSize.X = drawSize.X + fieldDelta.X;
                renderOffset.X = Math.Abs(fieldDelta.X) / 2;
            }
            if (fieldDelta.Y < 0)
            {
                drawSize.Y = drawSize.Y + fieldDelta.Y;
                renderOffset.Y = Math.Abs(fieldDelta.Y) / 2;
            }

                var curBackground = Console.BackgroundColor;
            for (int iy = 0; iy < drawSize.Y; iy++)
            for (int ix = 0; ix < drawSize.X * 2; ix++)
            {
                FieldNode node = field[(int)(drawModelFrom.X + ix * 0.5), drawModelFrom.Y + iy];

                int x = renderOffset.X + ix;
                int y = renderOffset.Y + iy;

                if (this.history[x, y] == node.Color)
                    continue;
                this.history[x, y] = node.Color;
                Console.BackgroundColor = node.Color;
                Console.SetCursorPosition(x, y);
                Console.Write(TransparentDecoration);
            }
            Console.BackgroundColor = curBackground; // fix for background color defaults getting messed up
        }

        void CheckViewportState()
        {
            if (Console.WindowWidth / 2 == this.viewport.X &&
                Console.WindowHeight == this.viewport.Y)
                return;

            Console.SetCursorPosition(0, 0); // if the cursor is outside the buffer before resizing the buffer, OutOfRange exception.
            Console.BufferHeight = Console.WindowHeight;
            Console.CursorVisible = false;

            this.viewport = new Xy(Console.WindowWidth / 2, Console.WindowHeight);
            this.history = new ConsoleColor[Console.WindowWidth, Console.WindowHeight];

            this.OnViewportChange?.Invoke();
        }
    }
}