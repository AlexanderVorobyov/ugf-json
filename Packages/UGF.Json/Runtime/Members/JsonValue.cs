using System;

namespace UGF.Json.Runtime.Members
{
    /// <summary>
    /// Represents part of Json structure as Value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public sealed class JsonValue<TValue> : IJsonMember
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public TValue Value { get; set; }
        
        public JsonMemberType MemberType { get { return JsonMemberType.Value; } }

        public JsonValue(TValue value = default(TValue))
        {
            Value = value;
        }
        
        public object GetValue()
        {
            return Value;
        }

        public void SetValue(object value)
        {
            Value = (TValue)value;
        }

        public Type GetValueType()
        {
            return Value.GetType();
        }

        public T FindValue<T>(string path, T defaultValue = default(T))
        {
            return defaultValue;
        }

        public IJsonMember Find(string path)
        {
            return null;
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}