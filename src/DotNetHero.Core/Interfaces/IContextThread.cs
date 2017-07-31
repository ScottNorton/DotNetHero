// Authored by Scott B. Norton

namespace DotNetHero.Core.Interfaces
{
    using System;
    using System.Threading;
    using DotNetHero.Core.Components;
    using DotNetHero.Core.Components.Threading;

    interface IContextThread
    {
        /// <summary>
        ///     Gets the thread id for this <see cref="ContextThread{T}" />
        /// </summary>
        int ThreadId { get; }

        /// <summary>
        ///     Returns true if within the <see cref="ContextThread{T}" /> managed <see cref="Thread" />.
        /// </summary>
        bool InThreadContext { get; }

        /// <summary>
        ///     Will enqueue a one-time-only action to <see cref="ContextThread{T}" /> for safety within parallelisation.
        /// </summary>
        /// <param name="action">A lambada expression or a method without argument or return value.</param>
        bool PostAsync(Action action);
    }
}