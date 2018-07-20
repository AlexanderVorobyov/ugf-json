using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UGF.Json.Runtime.Members;
using UnityEngine;

namespace UGF.Json.Runtime
{
    /// <summary>
    /// Provides methods for converting <see cref="IJsonMember"/> structure to Json text.
    /// </summary>
    public sealed class JsonWriter
    {
        private readonly StringBuilder m_builder = new StringBuilder();
        private readonly JsonFormatter m_formatter = new JsonFormatter();
        private readonly HashSet<object> m_references = new HashSet<object>();
        
        /// <summary>
        /// Writes member to Json text and returns it.
        /// </summary>
        /// <param name="member">The target member.</param>
        public string Write(IJsonMember member)
        {
            WriteMember(member);

            string result = m_builder.ToString();
            
            Reset();
            
            return result;
        }
        
        /// <summary>
        /// Resets the state of writer.
        /// </summary>
        public void Reset()
        {
            m_builder.Clear();
            m_formatter.Reset();
            m_references.Clear();
        }

        private void WriteMember(IJsonMember member)
        {
            switch (member.MemberType)
            {
                case JsonMemberType.Value:
                {
                    if (member is JsonNull)
                    {
                        WriteNumber("null");
                    }
                    else
                    {
                        var typeCode = Type.GetTypeCode(member.GetValueType());

                        switch (typeCode)
                        {
                            case TypeCode.Boolean:
                            {
                                var value = (JsonValue<bool>)member;
                                        
                                WriteNumber(value.Value ? bool.TrueString : bool.FalseString);
                                break;
                            }
                            case TypeCode.Byte:
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.SByte:
                            case TypeCode.Single:
                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                            {
                                string value = Convert.ToString(member.GetValue(), CultureInfo.InvariantCulture);

                                WriteNumber(value);
                                break;
                            }
                            case TypeCode.Char:
                            case TypeCode.String:
                            {
                                string value = Convert.ToString(member.GetValue(), CultureInfo.InvariantCulture);

                                WriteString(value);
                                break;
                            }
                            case TypeCode.DateTime:
                            case TypeCode.DBNull:
                            case TypeCode.Empty:
                            case TypeCode.Object:
                            {
                                Debug.LogWarningFormat("Unexpected member value type, member will be skipped: '{0}'.", member);
                                break;
                            }
                        }
                    }
                    break;
                }
                case JsonMemberType.Array:
                {
                    WriteArray((JsonArray)member);
                    break;
                }
                case JsonMemberType.Object:
                {
                    WriteObject((JsonObject)member);
                    break;
                }
            }
        }

        private void WriteObject(JsonObject member)
        {
            m_references.Add(member);
            m_builder.Append('{');

            int index = 0;

            foreach (var pair in member)
            {
                var element = pair.Value;

                if (m_references.Add(element))
                {
                    WriteString(pair.Key);

                    m_builder.Append(':');

                    WriteMember(element);

                    m_references.Remove(element);
                }
                else
                {
                    Debug.LogWarningFormat("Member circular references detected, member will be skipped: key '{0}'.", pair.Key);
                }

                if (index < member.Count - 1)
                {
                    m_builder.Append(',');
                }

                index++;
            }

            m_references.Remove(member);
            m_builder.Append('}');
        }

        private void WriteArray(JsonArray member)
        {
            m_references.Add(member);
            m_builder.Append('[');

            for (int i = 0; i < member.Count; i++)
            {
                var element = member[i];
                
                if (m_references.Add(element))
                {
                    WriteMember(element);

                    m_references.Remove(element);
                }
                else
                {
                    Debug.LogWarningFormat("Member circular references detected, member will be skipped: index '{0}'.", i);
                }
                
                if (i < member.Count - 1)
                {
                    m_builder.Append(',');
                }
            }

            m_references.Remove(member);
            m_builder.Append(']');
        }

        private void WriteNumber(string value)
        {
            m_builder.Append(value.ToLowerInvariant());
        }

        private void WriteString(string value)
        {
            m_builder.Append($"\"{m_formatter.Escape(value)}\"");
        }
    }
}