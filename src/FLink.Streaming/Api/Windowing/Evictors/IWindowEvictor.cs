﻿using System.Collections.Generic;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Runtime.Operators.Windowing;

namespace FLink.Streaming.Api.Windowing.Evictors
{
    /// <summary>
    /// An evictor can remove elements from a pane before/after the evaluation of WindowFunction and after the window evaluation gets triggered by a <see cref="WindowTrigger{T,TW}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements that this evictor can evict.</typeparam>
    /// <typeparam name="TW">The type of window on which this evictor can operate.</typeparam>
    public interface IWindowEvictor<T, TW> where TW : Window
    {
        /// <summary>
        /// Optionally evicts elements. Called before windowing function.
        /// </summary>
        /// <param name="elements">The elements currently in the window pane.</param>
        /// <param name="size">The current number of elements in the window pane.</param>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="evictorContext">The context for the Evictor.</param>
        void EvictBefore(IEnumerable<TimestampedValue<T>> elements, int size, TW window, IEvictorContext evictorContext);

        /// <summary>
        /// Optionally evicts elements. Called after windowing function.
        /// </summary>
        /// <param name="elements">The elements currently in the window pane.</param>
        /// <param name="size">The current number of elements in the window pane.</param>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="evictorContext">The context for the Evictor.</param>
        void EvictAfter(IEnumerable<TimestampedValue<T>> elements, int size, TW window, IEvictorContext evictorContext);

        /// <summary>
        ///  A context object that is given to evictor methods.
        /// </summary>
        interface IEvictorContext
        {
            /// <summary>
            /// The current processing time.
            /// </summary>
            long CurrentProcessingTime { get; }

            /// <summary>
            /// The current watermark time.
            /// </summary>
            long CurrentWatermark { get; }
        }
    }
}
