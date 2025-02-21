﻿using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Exceptions;
using System;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public class MissingTypeInfo : TypeInformation<InvalidTypesException>
    {
        public string FunctionName;
        public InvalidTypesException TypeException;

        public MissingTypeInfo(string functionName)
            : this(functionName, new InvalidTypesException("An unknown error occured."))
        {
        }

        public MissingTypeInfo(string functionName, InvalidTypesException typeException)
        {
            FunctionName = functionName;
            TypeException = typeException;
        }

        public override bool IsBasicType => throw new InvalidOperationException("The missing type information cannot be used as a type information.");

        public override bool IsTupleType => throw new InvalidOperationException("The missing type information cannot be used as a type information.");

        public override int Arity => throw new InvalidOperationException("The missing type information cannot be used as a type information.");

        public override int TotalFields => throw new InvalidOperationException("The missing type information cannot be used as a type information.");

        public override Type TypeClass => throw new InvalidOperationException("The missing type information cannot be used as a type information.");

        public override bool IsKeyType => throw new InvalidOperationException("The missing type information cannot be used as a type information.");

        public override TypeSerializer<InvalidTypesException> CreateSerializer(ExecutionConfig config) => throw new InvalidOperationException("The missing type information cannot be used as a type information.");

        public override string ToString() => GetType().Name + "<" + FunctionName + ", " + TypeException.Message + ">";

        public override bool Equals(object obj) => obj is MissingTypeInfo;

        public override int GetHashCode() => 31 * FunctionName.GetHashCode() + TypeException.GetHashCode();
    }
}
