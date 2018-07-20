using System;
using JetBrains.Annotations;

namespace UGF.Json.Runtime.Members
{
    /// <summary>
    /// Provides utilities for working with <see cref="IJsonMember"/>.
    /// </summary>
    public static class JsonMemberUtility
    {
        /// <summary>
        /// Finds the value of member by specified path.
        /// <para>If member cannot be found or value cannot be converted to specified type, returns default value.</para>
        /// </summary>
        /// <typeparam name="T">The target type of value.</typeparam>
        /// <param name="member">The target member for search.</param>
        /// <param name="path">The path for search of member.</param>
        /// <param name="defaultValue">The default value, if find was failed.</param>
        public static T FindValue<T>(IJsonMember member, string path, T defaultValue = default(T))
        {
            var target = Find(member, path);

            if (target != null)
            {
                try
                {
                    return (T)Convert.ChangeType(target.GetValue(), typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Finds the member by specified path. (Can be Null)
        /// </summary>
        /// <param name="member">The target member for search.</param>
        /// <param name="path">The path for search of member.</param>
        [CanBeNull]
        public static IJsonMember Find(IJsonMember member, string path)
        {
            var names = path.Split('.');
            var target = member;

            for (int i = 0; i < names.Length; i++)
            {
                if (target.MemberType == JsonMemberType.Array || target.MemberType == JsonMemberType.Object)
                {
                    target = FindElementInCollection(target, names[i]);

                    if (target == null)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            return target;
        }

        private static IJsonMember FindElementInCollection(IJsonMember member, string indexOrKey)
        {
            var array = member as JsonArray;

            if (array != null)
            {
                int index;

                if (int.TryParse(indexOrKey, out index))
                {
                    return array[index];
                }
            }

            var dictionary = member as JsonObject;

            if (dictionary != null)
            {
                IJsonMember element;

                if (dictionary.TryGetValue(indexOrKey, out element))
                {
                    return dictionary[indexOrKey];
                }
            }

            return null;
        }
    }
}