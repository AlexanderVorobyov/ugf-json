using System;
using System.Globalization;
using System.Text;

namespace UGF.Json.Runtime
{
    /// <summary>
    /// Provides methods for formatting Json to compact or readable layout.
    /// </summary>
    public sealed class JsonFormatter
    {
        private readonly JsonTextReader m_reader = new JsonTextReader();
        private readonly StringBuilder m_builder = new StringBuilder();
        
        /// <summary>
        /// Resets the state of formatter.
        /// </summary>
        public void Reset()
        {
            m_reader.Reset();
            m_builder.Clear();
        }

        /// <summary>
        /// Converts specified text to compact layout.
        /// </summary>
        /// <param name="text">The Json text.</param>
        public string ToCompact(string text)
        {
            m_reader.Setup(text, 0, text.Length);
            m_reader.SkipWhiteSpace();

            while (m_reader.CanRead())
            {
                char ch = m_reader.Read();

                switch (ch)
                {
                    case '\"':
                    {
                        m_builder.Append($"\"{m_reader.ReadUntil('\"')}\"");
                        m_reader.Read();
                        break;
                    }
                    case '\b':
                    case '\f':
                    case '\n':
                    case '\r':
                    case '\t':
                    {
                        break;
                    }
                    default:
                    {
                        m_builder.Append(ch);
                        break;
                    }
                }
                
                m_reader.SkipWhiteSpace();
            }

            string result = m_builder.ToString();
            
            Reset();

            return result;
        }

        /// <summary>
        /// Convert text to readable layout.
        /// </summary>
        /// <param name="text">The Json text.</param>
        /// <param name="indent">The indent. Default is 4 spaces.</param>
        public string ToReadable(string text, string indent = "    ")
        {
            m_reader.Setup(text, 0, text.Length);

            int depth = 0;

            while (m_reader.CanRead())
            {
                char ch = m_reader.Read();

                switch (ch)
                {
                    case '\"':
                    {
                        m_builder.Append($"\"{m_reader.ReadUntil('\"')}\"");
                        m_reader.Read();
                        break;
                    }
                    case '{':
                    case '[':
                    {
                        m_builder.Append(ch);
                        m_builder.AppendLine();

                        AddIndent(indent, ++depth, m_builder);
                        break;
                    }
                    case '}':
                    case ']':
                    {
                        m_builder.AppendLine();
                        
                        AddIndent(indent, --depth, m_builder);

                        m_builder.Append(ch);
                        break;
                    }
                    case ',':
                    {
                        m_builder.Append(ch);
                        m_builder.AppendLine();

                        AddIndent(indent, depth, m_builder);
                        break;
                    }
                    case ':':
                    {
                        m_builder.Append(": ");
                        break;
                    }
                    default:
                    {
                        m_builder.Append(ch);
                        break;
                    }
                }
                
                m_reader.SkipWhiteSpace();
            }
            
            string result = m_builder.ToString();
            
            Reset();

            return result;
        }
        
        /// <summary>
        /// Escapes the specified text.
        /// </summary>
        /// <param name="text">The text to escape.</param>
        public string Escape(string text)
        {
            m_reader.Setup(text, 0, text.Length);

            while (m_reader.CanRead())
            {
                char ch = m_reader.Read();

                switch (ch)
                {
                    case '"':
                    {
                        m_builder.Append("\\\"");
                        break;
                    }
                    case '\\':
                    {
                        m_builder.Append("\\\\");
                        break;
                    }
                    case '\b':
                    {
                        m_builder.Append("\\b");
                        break;
                    }
                    case '\f':
                    {
                        m_builder.Append("\\f");
                        break;
                    }
                    case '\n':
                    {
                        m_builder.Append("\\n");
                        break;
                    }
                    case '\r':
                    {
                        m_builder.Append("\\r");
                        break;
                    }
                    case '\t':
                    {
                        m_builder.Append("\\t");
                        break;
                    }
                    default:
                    {
                        if (ch > 127)
                        {
                            int code = ch;

                            m_builder.Append($"\\u{code:x4}");
                        }
                        else
                        {
                            m_builder.Append(ch);   
                        }
                        break;
                    }
                }
            }
            
            string result = m_builder.ToString();
            
            Reset();

            return result;
        }

        /// <summary>
        /// Unescapes the specified text.
        /// </summary>
        /// <param name="text">The text to unescape.</param>
        public string Unescape(string text)
        {
            m_reader.Setup(text, 0, text.Length);

            while (m_reader.CanRead())
            {
                char ch = m_reader.Read();

                if (ch == '\\')
                {
                    ch = m_reader.Read();

                    switch (char.ToLower(ch))
                    {
                        case '"':
                        case '\\':
                        case '/':
                        {
                            m_builder.Append(ch);
                            break;
                        }
                        case 'b':
                        {
                            m_builder.Append('\b');
                            break;
                        }
                        case 'f':
                        {
                            m_builder.Append('\f');
                            break;
                        }
                        case 'n':
                        {
                            m_builder.Append('\n');
                            break;
                        }
                        case 'r':
                        {
                            m_builder.Append('\r');
                            break;
                        }
                        case 't':
                        {
                            m_builder.Append('\t');
                            break;
                        }
                        case 'u':
                        {
                            int code = int.Parse(m_reader.Read(4), NumberStyles.HexNumber);

                            m_builder.Append((char)code);
                            break;
                        }
                        default:
                        {
                            throw new Exception("Unexpected symbol for escape.");
                        }
                    }
                }
                else
                {
                    m_builder.Append(ch);
                }
            }

            string result = m_builder.ToString();
            
            Reset();

            return result;
        }
        
        private void AddIndent(string indent, int count, StringBuilder builder)
        {
            for (int i = 0; i < count; i++)
            {
                builder.Append(indent);
            }
        }
    }
}