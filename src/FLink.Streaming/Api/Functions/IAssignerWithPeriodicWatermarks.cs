﻿using FLink.Streaming.Api.Watermarks;

namespace FLink.Streaming.Api.Functions
{
    /// <summary>
    /// Assigns event time timestamps to elements, and generates low watermarks that signal event time progress within the stream.
    /// These timestamps and watermarks are used by functions and operators that operate on event time, for example event time windows.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements to which this assigner assigns timestamps.</typeparam>
    public interface IAssignerWithPeriodicWatermarks<in TElement> : ITimestampAssigner<TElement>
    {
        /// <summary>
        /// Generate watermark. This method is periodically called by the system to retrieve the current watermark. if no watermark should be emitted, or the next watermark to emit.
        /// </summary>
        Watermark CurrentWatermark { get; }
    }
}
