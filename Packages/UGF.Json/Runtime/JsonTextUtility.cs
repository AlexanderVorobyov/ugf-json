using UGF.Json.Runtime.Members;

namespace UGF.Json.Runtime
{
    /// <summary>
    /// Provides utilities for working with Json text.
    /// </summary>
    public static class JsonTextUtility
    {
        /// <summary>
        /// Writes member to Json text and returns it.
        /// </summary>
        /// <param name="member">The target member.</param>
        /// <param name="readable">If true, format to readable layout. Otherwise used compact layout. Default is false.</param>
        public static string Write(IJsonMember member, bool readable = false)
        {
            var writer = new JsonWriter();
            string result = writer.Write(member);

            if (readable)
            {
                result = ToReadable(result);
            }

            return result;
        }
        
        /// <summary>
        /// Reads Json text and create <see cref="IJsonMember"/> structure.
        /// <para>Input text can be any Json value: string, value, array or object.</para>
        /// </summary>
        /// <param name="text">The Json text to read.</param>
        public static IJsonMember Read(string text)
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
        public static IJsonMember Read(string text, int position, int length)
        {
            var reader = new JsonReader();
            var member = reader.Read(text, position, length);

            reader.Reset();

            return member;
        }
        
        /// <summary>
        /// Converts specified text to compact layout.
        /// </summary>
        /// <param name="text">The Json text.</param>
        public static string ToCompact(string text)
        {
            var formatter = new JsonFormatter();

            return formatter.ToCompact(text);
        }
        
        /// <summary>
        /// Convert text to readable layout.
        /// </summary>
        /// <param name="text">The Json text.</param>
        /// <param name="indent">The indent. Default is 4 spaces.</param>
        public static string ToReadable(string text, string indent = "    ")
        {
            var formatter = new JsonFormatter();

            return formatter.ToReadable(text, indent);
        }

        /// <summary>
        /// Escapes the specified text.
        /// </summary>
        /// <param name="text">The text to escape.</param>
        public static string Escape(string text)
        {
            var formatter = new JsonFormatter();

            return formatter.Escape(text);
        }

        /// <summary>
        /// Unescapes the specified text.
        /// </summary>
        /// <param name="text">The text to unescape.</param>
        public static string Unescape(string text)
        {
            var formatter = new JsonFormatter();

            return formatter.Unescape(text);
        }

        /// <summary>
        /// Determines whether the specified text is Json number.
        /// </summary>
        /// <param name="text">The text to check.</param>
        public static bool IsNumber(string text)
        {
            if (text.ToLowerInvariant() == "false"
                || text.ToLowerInvariant() == "true"
                || text.ToLowerInvariant() == "null")
            {
                return true;
            }

            bool wasDot = false;
            bool wasE = false;
            bool wasSign = false;

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                
                if (!char.IsDigit(ch))
                {
                    if (i == 0)
                    {
                        if (ch == '-')
                        {
                            continue;
                        }

                        return false;
                    }

                    if (i == text.Length - 1)
                    {
                        return false;
                    }

                    switch (ch)
                    {
                        case '.':
                        {
                            if (wasDot || wasE)
                            {
                                return false;
                            }

                            wasDot = true;
                            continue;
                        }
                        case 'e':
                        {
                            if (wasE)
                            {
                                return false;
                            }

                            wasE = true;
                            continue;
                        }
                        case '-':
                        case '+':
                        {
                            if (!wasE || wasSign)
                            {
                                return false;
                            }

                            wasSign = true;
                            break;
                        }
                        default:
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}