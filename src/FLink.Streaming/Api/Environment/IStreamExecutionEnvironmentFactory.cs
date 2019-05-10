﻿namespace FLink.Streaming.Api.Environment
{
    /// <summary>
    /// Factory class for stream execution environments.
    /// </summary>
    public interface IStreamExecutionEnvironmentFactory
    {
        /// <summary>
        /// Creates a StreamExecutionEnvironment from this factory.
        /// </summary>
        /// <returns>A <see cref="StreamExecutionEnvironment"/>.</returns>
        StreamExecutionEnvironment CreateExecutionEnvironment();
    }
}
