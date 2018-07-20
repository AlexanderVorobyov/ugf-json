using System;
using System.Collections.Generic;

namespace UGF.Json.Runtime.Members
{
    /// <summary>
    /// Represents part of Json structure as Dictionary of Json members.
    /// </summary>
    public sealed class JsonObject : Dictionary<string, IJsonMember>, IJsonMember
    {
        public JsonMemberType MemberType { get { return JsonMemberType.Object; } }

        public object GetValue()
        {
            return this;
        }

        public void SetValue(object value)
        {
        }

        public Type GetValueType()
        {
            return GetType();
        }

        public T FindValue<T>(string path, T defaultValue = default(T))
        {
            return JsonMemberUtility.FindValue(this, path, defaultValue);
        }

        public IJsonMember Find(string path)
        {
            return JsonMemberUtility.Find(this, path);
        }
    }
}