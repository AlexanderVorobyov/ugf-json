using System;

namespace UGF.Json.Runtime.Members
{
    /// <summary>
    /// Represents part of Json structure as Null value.
    /// </summary>
    public sealed class JsonNull : IJsonMember
    {
        public JsonMemberType MemberType { get { return JsonMemberType.Value; } }

        public object GetValue()
        {
            return null;
        }

        public void SetValue(object value)
        {
        }

        public Type GetValueType()
        {
            return null;
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
            return "null";
        }
    }
}