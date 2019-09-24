﻿using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Streaming.Api.DataStream;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Transformations;

namespace FLink.Streaming.Api.Environment
{
    /// <summary>
    /// The StreamExecutionEnvironment is the context in which a streaming program is executed. 
    /// </summary>
    /// <remarks>
    /// The environment provides methods to control the job execution (such as setting the parallelism or the fault tolerance/check pointing parameters) and to interact with the outside world(data access).
    /// </remarks>
    public abstract class StreamExecutionEnvironment
    {
        // The default name to use for a streaming job if no other name has been specified.
        public const string DefaultJobName = "Flink Streaming Job";

        // The time characteristic that is used if none other is set.
        private const TimeCharacteristic DefaultTimeCharacteristic = TimeCharacteristic.ProcessingTime;

        // The default buffer timeout (max delay of records in the network stack).
        private const long DefaultNetworkBufferTimeout = 100L;

        // The environment of the context (local by default, cluster if invoked through command line).
        private static IStreamExecutionEnvironmentFactory _contextEnvironmentFactory;

        // The default parallelism used when creating a local environment.
        private static int _defaultLocalParallelism = System.Environment.ProcessorCount;

        // The execution configuration for this environment.
        private static readonly ExecutionConfig Config = new ExecutionConfig();

        private readonly List<StreamTransformation<object>> _transformations = new List<StreamTransformation<object>>();

        private long _bufferTimeout = DefaultNetworkBufferTimeout;

        protected bool IsChainingEnabled = true;

        public DataStreamSource<TOut> FromCollection<TOut>(IEnumerable<TOut> data, TypeInformation<TOut> typeInfo)
        {
            Preconditions.CheckNotNull(data, "Collection must not be null");

            // must not have null elements and mixed elements
            FromElementsFunction<TOut>.CheckCollection(data, typeInfo.TypeClass);

            ISourceFunction<TOut> function;
            try
            {
                function = new FromElementsFunction<TOut>(typeInfo.CreateSerializer(Config), data);
            }
            catch (Exception e)
            {
                throw new RuntimeException(e.Message, e);
            }

            return AddSource(function, "Collection Source", typeInfo)
                .SetParallelism(1);
        }

        public DataStreamSource<TOut> AddSource<TOut>(ISourceFunction<TOut> function, string sourceName, TypeInformation<TOut> typeInfo = default)
        {
            var isParallel = function is IParallelSourceFunction<TOut>;

            Clean(function);

            var sourceOperator = new StreamSource<TOut, ISourceFunction<TOut>>(function);
            return new DataStreamSource<TOut>(this, typeInfo, sourceOperator, isParallel, sourceName);
        }


        public int Parallelism { get; } = Config.GetParallelism();

        public T Clean<T>(T t)
        {
            return t;
        }

        /// <summary>
        /// Gets the config object. 
        /// </summary>
        /// <returns></returns>
        public ExecutionConfig GetConfig()
        {
            return Config;
        }

        public StreamExecutionEnvironment SetParallelism(int parallelism)
        {
            Config.SetParallelism(parallelism);
            return this;
        }
    }
}
