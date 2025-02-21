﻿namespace FLink.Core.Api.Common.States
{
    /// <summary>
    /// <see cref="IState"/> interface for reducing state.
    /// Elements can be added to the state, they will be combined using a reduce function. The current state can be inspected.
    /// </summary>
    /// <typeparam name="TValue">Type of the value in the operator state</typeparam>
    public interface IReducingState<TValue> : IMergingState<TValue, TValue> { }
}
