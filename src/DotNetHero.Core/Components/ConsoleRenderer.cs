// Authored by Scott B. Norton

namespace DotNetHero.Core.Components
{
    using System;
    using DotNetHero.Core.Geometry;
    using DotNetHero.Core.Structures;

    public sealed class ConsoleRenderer : ContextThread<ConsoleRenderer>
    {
        const char TransparentDecoration = ' ';

        ConsoleColor[,] history;
        Xy viewport;

        ConsoleRenderer()
        {
        }

        public event Action OnViewportChange;

        /// <summary>
        /// Draws a game field to a console window.
        /// </summary>
        /// <remarks>
        /// This will take some extra time to distill before it's complete, will come back to it later.
        /// Need to keep execution of redraws (stide drawing via history) close to 30ms as possible to keep motion smooth.
        /// todo distil math for focal point drawing, viewport occlusion, and centering - no rectangle clipping allowed!
        /// todo figure out why the buffer is drawn one under size.
        /// todo dedicated console draw thread - done
        /// </remarks>
        public void Draw(GameField field, Xy focusPoint)
        {
            if (!this.InThreadContext)
            {
                this.PostAsync(() => this.Draw(field, focusPoint));
                return;
            }

            this.CheckViewportState();

            Xy drawModelFrom = Xy.Empty;
            if (focusPoint.X >= this.viewport.X / 2)
                drawModelFrom.X = focusPoint.X - this.viewport.X / 2;
            if (focusPoint.Y >= this.viewport.Y / 2)
                drawModelFrom.Y = focusPoint.Y / 2;

            var drawSize = new Xy(
                (int)Math.Ceiling(field.Size.X * (1f / field.Size.X * this.viewport.X)),
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

            for (int iy = 0; iy < drawSize.Y; iy++)
            for (int ix = 0; ix < drawSize.X * 2; ix++)
            {
                ConsoleColor renderColor = field[(int)(drawModelFrom.X + ix * 0.5), drawModelFrom.Y + iy];
                int x = renderOffset.X + ix;
                int y = renderOffset.Y + iy;

                if (this.history[x, y] == renderColor)
                    continue;
                this.history[x, y] = renderColor;

                Console.BackgroundColor = renderColor;
                Console.SetCursorPosition(x, y);
                Console.Write(TransparentDecoration);
            }
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