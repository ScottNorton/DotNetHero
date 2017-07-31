// Authored by Scott B. Norton

namespace DotNetHero.Core.Components.Threading
{
    using System;
    using System.Threading;
    using DotNetHero.Core.Interfaces;

    /// <summary>
    /// Creates a shortcut for the <see cref="Thread"/> queue of <see cref="ContextThread{TContext}"/> in another class.
    /// </summary>
    abstract class ContextThreadProxy<TContext, TProxy> : IContextThread
        where TContext : ContextThread<TContext>
        where TProxy : ContextThreadProxy<TContext, TProxy>
    {
        public int ThreadId => ContextThread<TContext>.Instance.ThreadId;

        public bool InThreadContext => ContextThread<TContext>.Instance.InThreadContext;

        public virtual bool PostAsync(Action action)
        {
            return ContextThread<TContext>.Instance.PostAsync(action);
        }
    }
}