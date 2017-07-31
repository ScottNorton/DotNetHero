// Authored by Scott B. Norton

namespace DotNetHero.Core.Components
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    /// <summary>
    /// Asynchronous, contextual, object-oriented threading implementation.
    /// </summary>
    /// <remarks>
    /// Can produce synchronous invocation.
    /// </remarks>
    public abstract class ContextThread<T>
        where T : ContextThread<T>
    {
        readonly ConcurrentQueue<Action> actionQueue;
        readonly Thread thread;
        readonly SemaphoreSlim workBlock;

        /// <summary>
        /// The <see cref="Action"/> object currently in invocation.
        /// </summary>
        Action activeAction;

        protected ContextThread()
        {
            this.workBlock = new SemaphoreSlim(0);
            this.actionQueue = new ConcurrentQueue<Action>();
            this.thread = new Thread(this.ThreadBody)
            {
                Name = typeof(T).Name
            };

            this.thread.Start();
        }

        public int ThreadId => this.thread.ManagedThreadId;

        public bool InThreadContext => this.ThreadId == Thread.CurrentThread.ManagedThreadId;

        /// <summary>
        /// Enqueues a one-time method invocation to the <see cref="Thread"/> of <see cref="ContextThread{T}"/> asynchronously.
        /// </summary>
        /// <returns>True if completed synchronously.</returns>
        public bool PostAsync(Action action)
        {
            if (action == null)
                throw new ArgumentException("Value cannot be null", nameof(action));

            this.actionQueue.Enqueue(action);
            this.workBlock.Release(1);

            return !this.actionQueue.TryPeek(out Action qAction) || qAction != action;
        }

        void ThreadBody()
        {
            for (;; this.workBlock.Wait())
                while (this.actionQueue.Count > 0 && this.actionQueue.TryDequeue(out this.activeAction))
                    this.TryInvokeActive();
        }

        void TryInvokeActive()
        {
            try
            {
                this.activeAction();
                this.activeAction = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}