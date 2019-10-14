﻿using System;
using FLink.Core.Api.Common.State;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// A Trigger determines when a pane of a window should be evaluated to emit the results for that part of the window.
    /// </summary>
    /// <typeparam name="TElement">The type of elements on which this Trigger works.</typeparam>
    /// <typeparam name="TWindow">The type of window on which this trigger can operate.</typeparam>
    public abstract class WindowTrigger<TElement, TWindow> where TWindow : Window
    {
        /// <summary>
        /// Called for every element that gets added to a pane. The result of this will determine whether the pane is evaluated to emit results.
        /// This method is not called in case the window does not contain any elements.
        /// </summary>
        /// <param name="element">The element that arrived.</param>
        /// <param name="timestamp">The timestamp of the element that arrived.</param>
        /// <param name="window">The window to which the element is being added.</param>
        /// <param name="ctx">A context object that can be used to register timer callbacks.</param>
        /// <returns></returns>
        public abstract WindowTriggerResult OnElement(TElement element, long timestamp, TWindow window, ITriggerContext ctx);

        /// <summary>
        /// Called when a processing-time timer that was set using the trigger context fires.
        /// This method is not called in case the window does not contain any elements.
        /// </summary>
        /// <param name="time">The timestamp at which the timer fired.</param>
        /// <param name="window">The window for which the timer fired.</param>
        /// <param name="ctx">A context object that can be used to register timer callbacks.</param>
        /// <returns></returns>
        public abstract WindowTriggerResult OnProcessingTime(long time, TWindow window, ITriggerContext ctx);

        /// <summary>
        /// Called when an event-time timer that was set using the trigger context fires.
        /// Note: This method is not called in case the window does not contain any elements.
        /// </summary>
        /// <param name="time">The timestamp at which the timer fired.</param>
        /// <param name="window">The window for which the timer fired.</param>
        /// <param name="ctx">A context object that can be used to register timer callbacks.</param>
        /// <returns></returns>
        public abstract WindowTriggerResult OnEventTime(long time, TWindow window, ITriggerContext ctx);

        /// <summary>
        /// Returns true if this trigger supports merging of trigger state and can therefore be used with a <see cref="MergingWindowAssigner{T,TW}"/>
        /// </summary>
        public virtual bool CanMerge => false;

        /// <summary>
        /// Called when several windows have been merged into one window by the <see cref="WindowAssigner{TElement,TWindow}"/>.
        /// </summary>
        /// <param name="window">The new window that results from the merge.</param>
        /// <param name="ctx">A context object that can be used to register timer callbacks and access state.</param>
        public virtual void OnMerge(TWindow window, IOnMergeContext ctx) => throw new InvalidOperationException("This trigger does not support merging.");

        /// <summary>
        /// Clears any state that the trigger might still hold for the given window. This is called when a window is purged.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="ctx"></param>
        public abstract void clear(TWindow window, ITriggerContext ctx);

        /// <summary>
        /// A context object that is given to trigger methods to allow them to register timer callbacks and deal with state.
        /// </summary>
        public interface ITriggerContext
        {
            /// <summary>
            /// Returns the current processing time.
            /// </summary>
            long CurrentProcessingTime { get; }

            /// <summary>
            /// Returns the current watermark time.
            /// </summary>
            long CurrentWatermark { get; }

            /// <summary>
            /// Register a system time callback.
            /// </summary>
            /// <param name="time">The time at which to invoke <see cref="WindowTrigger{T,TW}.OnProcessingTime"/></param>.
            void RegisterProcessingTimeTimer(long time);

            /// <summary>
            /// Register an event-time callback.
            /// </summary>
            /// <param name="time">The watermark at which to invoke <see cref="WindowTrigger{T,TW}.OnEventTime"/></param></param>
            void RegisterEventTimeTimer(long time);

            /// <summary>
            /// Delete the processing time trigger for the given time.
            /// </summary>
            /// <param name="time"></param>
            void DeleteProcessingTimeTimer(long time);

            /// <summary>
            /// Delete the event-time trigger for the given time.
            /// </summary>
            /// <param name="time"></param>
            void DeleteEventTimeTimer(long time);
        }

        public interface IOnMergeContext : ITriggerContext
        {
            void MergePartitionedState<TState>(StateDescriptor<TState, object> stateDescriptor)
                where TState : IMergingState<object, object>;
        }
    }
}
