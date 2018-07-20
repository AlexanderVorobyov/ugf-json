using System;
using JetBrains.Annotations;

namespace UGF.Json.Runtime.Members
{
    /// <summary>
    /// Represents part of Json structure, which can be a value, null-value, array or object.
    /// </summary>
    public interface IJsonMember
    {
        /// <summary>
        /// Gets the type of the member, which can be: Value, Array or Object. (Read Only)
        /// </summary>
        JsonMemberType MemberType { get; }

        /// <summary>
        /// If this member can contains the value, gets the value, otherwise returns himself as value.
        /// </summary>
        object GetValue();

        /// <summary>
        /// If this member can contains the value, sets the new value, otherwise this method no effect.
        /// <para>Can throw an exception, if specified value cannot be converted to value type of this member.</para>
        /// </summary>
        /// <param name="value">The new value.</param>
        void SetValue(object value);

        /// <summary>
        /// If this member can contains the value, gets the value type, otherwise returns the type of himself.
        /// </summary>
        Type GetValueType();

        /// <summary>
        /// Finds the value of member by specified path.
        /// <para>If member cannot be found or value cannot be converted to specified type, returns default value.</para>
        /// </summary>
        /// <typeparam name="T">The target type of value.</typeparam>
        /// <param name="path">The path for search of member.</param>
        /// <param name="defaultValue">The default value, if find was failed.</param>
        T FindValue<T>(string path, T defaultValue = default(T));

        /// <summary>
        /// Finds the member by specified path. (Can be Null)
        /// </summary>
        /// <param name="path">The path for search of member.</param>
        [CanBeNull]
        IJsonMember Find(string path);
    }
}