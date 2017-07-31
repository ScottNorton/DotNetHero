// Authored by Scott B. Norton

namespace DotNetHero.Core.Components.Threading
{
    using System;
    using System.Threading;

    /// <summary>
    /// Creates a <see cref="Timer"/>. The callback is executed on <see cref="ContextThread{T}"/>
    /// </summary>
    sealed class ContextTimer<T> : ContextThreadProxy<T, ContextTimer<T>>, IDisposable
        where T : ContextThread<T>
    {
        readonly Timer timer;
        readonly int requestedIterations;
        int completedIterations;

        event Action Callback;

        public ContextTimer(Action callback, TimeSpan delayedStart, TimeSpan interval, int interations = 0)
        {
            this.Callback = callback;
            this.requestedIterations = interations;
            this.timer = new Timer(this.CallbackInternal, null, delayedStart, interval);
        }

        void CallbackInternal(object cookie)
        {
            this.PostAsync(this.Callback); // todo iterate invocation list instead for better exception handling

            if (this.requestedIterations > 0 && this.requestedIterations < ++this.completedIterations)
                this.Dispose();
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }

        /// <summary>
        /// Subscribes an action to <see cref="ContextTimer{T}"/> to be run on <see cref="ContextThread{T}"/>
        /// </summary>
        public void Subscribe(Action action)
        {
            this.Callback += action;
        }

        /// <summary>
        /// Unsubscribes an action from <see cref="ContextTimer{T}"/>
        /// </summary>
        public void Unsubscribe(Action action)
        {
            this.Callback -= action;
        }
    }
}