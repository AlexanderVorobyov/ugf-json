using System;
using System.Globalization;
using System.Text;
using UGF.Json.Runtime.Members;

namespace UGF.Json.Runtime
{
    /// <summary>
    /// Provides methods for converting Json text to <see cref="IJsonMember"/> structure.
    /// </summary>
    public sealed class JsonReader
    {
        private readonly JsonTextReader m_reader = new JsonTextReader();
        private readonly JsonFormatter m_formatter = new JsonFormatter();
        private readonly StringBuilder m_builder = new StringBuilder();
        
        /// <summary>
        /// Reads Json text and create <see cref="IJsonMember"/> structure.
        /// <para>Input text can be any Json value: string, value, array or object.</para>
        /// </summary>
        public IJsonMember Read(string text)
        {
            return Read(text, 0, text.Length);
        }

        /// <summary>
        /// Reads Json text and create <see cref="IJsonMember"/> structure.
        /// <para>Input text can be any Json value: string, value, array or object.</para>
        /// </summary>
        /// <param name="text">The Json text to read.</param>
        /// <param name="position">The start position in text. The position cannot be less than zero and more than length.</param>
        /// <param name="length">The length for reading. The length cannot be less than zero and more than text length.</param>
        public IJsonMember Read(string text, int position, int length)
        {
            m_reader.Setup(text, position, length);

            var member = Read();

            Reset();

            return member;
        }
        
        /// <summary>
        /// Resets the state of reader.
        /// </summary>
        public void Reset()
        {
            m_reader.Reset();
            m_formatter.Reset();
            m_builder.Clear();
        }

        private IJsonMember Read()
        {
            m_reader.SkipWhiteSpace();

            char value = m_reader.Peek();

            if (char.IsNumber(value))
            {
                return ReadDouble();
            }

            switch (value)
            {
                case '{': return ReadObject();
                case '[': return ReadArray();
                case '"': return new JsonValue<string>(ReadString());
                case '-': return ReadDouble();
                case 't': return ReadBoolean(true);
                case 'f': return ReadBoolean(false);
                case 'n': return new JsonNull();
            }
            
            throw new Exception($"Unexpected symbol: '{value}' ({m_reader.Position}).");
        }

        private JsonArray ReadArray()
        {
            var member = new JsonArray();

            m_reader.Read();
            m_reader.SkipWhiteSpace();

            if (m_reader.Peek() == ']')
            {
                m_reader.Read();
            }
            else
            {
                while (true)
                {
                    m_reader.SkipWhiteSpace();

                    member.Add(Read());

                    m_reader.SkipWhiteSpace();

                    if (m_reader.Read() == ']')
                    {
                        break;
                    }
                }
            }

            return member;
        }

        private JsonObject ReadObject()
        {
            var member = new JsonObject();

            m_reader.Read();
            m_reader.SkipWhiteSpace();

            if (m_reader.Peek() == '}')
            {
                m_reader.Read();
            }
            else
            {
                while (true)
                {
                    m_reader.SkipWhiteSpace();

                    string key = ReadString();

                    m_reader.SkipWhiteSpace();
                    m_reader.Read();
                    m_reader.SkipWhiteSpace();

                    member.Add(key, Read());

                    m_reader.SkipWhiteSpace();

                    if (m_reader.Read() == '}')
                    {
                        break;
                    }
                }
            }

            return member;
        }

        private JsonValue<double> ReadDouble()
        {
            string value = ReadNumber();
            
            return new JsonValue<double>(double.Parse(value, CultureInfo.InvariantCulture));
        }

        private JsonValue<bool> ReadBoolean(bool state)
        {
            m_reader.Read(state ? 4 : 5);

            return new JsonValue<bool>(state);
        }

        private string ReadNumber()
        {
            if (m_reader.Peek() == '-')
            {
                m_builder.Append(m_reader.Read());
            }

            if (m_reader.Peek() == '0')
            {
                m_builder.Append(m_reader.Read());
            }
            else
            {
                m_builder.Append(m_reader.ReadUntil(ch => !char.IsDigit(ch)));
            }

            if (m_reader.CanRead() && m_reader.Peek() == '.')
            {
                m_builder.Append(m_reader.Read());
                m_builder.Append(m_reader.ReadUntil(ch => !char.IsDigit(ch)));
            }

            if (m_reader.CanRead() && char.ToLowerInvariant(m_reader.Peek()) == 'e')
            {
                m_builder.Append(m_reader.Read());
                m_builder.Append(m_reader.Read());
                m_builder.Append(m_reader.ReadUntil(ch => !char.IsDigit(ch)));
            }

            string result = m_builder.ToString();

            m_builder.Clear();

            return result;
        }

        private string ReadString()
        {
            m_reader.Read();

            string result = m_formatter.Unescape(m_reader.ReadUntil('\"'));
            
            m_reader.Read();

            return result;
        }
    }
}