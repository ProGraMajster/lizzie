using System;
using System.Collections.Generic;

namespace lizzie.Runtime
{
    public enum ScriptValueType
    {
        Number,
        Bool,
        String,
        Array,
        Map,
        Function,
        Null,
        ObjectRef
    }

    public readonly record struct ScriptValue
    {
        public ScriptValueType Type { get; }
        private readonly object? _value;

        private ScriptValue(ScriptValueType type, object? value)
        {
            Type = type;
            _value = value;
        }

        public object? Value => _value;

        public static ScriptValue FromNumber(double value) => new(ScriptValueType.Number, value);
        public static ScriptValue FromBool(bool value) => new(ScriptValueType.Bool, value);
        public static ScriptValue FromString(string value) => new(ScriptValueType.String, value);
        public static ScriptValue FromArray(IReadOnlyList<ScriptValue> value) => new(ScriptValueType.Array, value);
        public static ScriptValue FromMap(IReadOnlyDictionary<string, ScriptValue> value) => new(ScriptValueType.Map, value);
        public static ScriptValue FromFunction(Delegate value) => new(ScriptValueType.Function, value);
        public static ScriptValue FromObjectRef(int handle) => new(ScriptValueType.ObjectRef, handle);
        public static ScriptValue Null => new(ScriptValueType.Null, null);

        public T? As<T>() => (T?)_value;
    }
}
