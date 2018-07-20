using System;
using System.Collections.Generic;

namespace UGF.Json.Runtime.Members
{
    /// <summary>
    /// Represents part of Json structure as Array of Json members.
    /// </summary>
    public sealed class JsonArray : List<IJsonMember>, IJsonMember
    {
        public JsonMemberType MemberType { get { return JsonMemberType.Array; } }
        
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