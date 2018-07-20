using System;
using System.Text;
using UnityEngine.Assertions;

namespace UGF.Json.Runtime
{
    /// <summary>
    /// Provides methods for reading text stream.
    /// </summary>
    internal sealed class JsonTextReader
    {
        /// <summary>
        /// Gets the text. (Read Only)
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the current position. (Read Only)
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Gets the length. (Read Only)
        /// </summary>
        public int Length { get; private set; }

        private readonly StringBuilder m_builder = new StringBuilder();
        
        /// <summary>
        /// Setups the specified text to read and parameters for start position and length.
        /// </summary>
        /// <param name="text">The text to read.</param>
        /// <param name="position">The start position in text. The position cannot be less than zero and more than length.</param>
        /// <param name="length">The length for reading. The length cannot be less than zero and more than text length.</param>
        public void Setup(string text, int position, int length)
        {
            Assert.IsTrue(position >= 0 && position < length, "The position cannot be less than zero and more than length.");
            Assert.IsTrue(length >= 0 && length <= text.Length, "The length cannot be less than zero and more than text length.");
            
            Reset();

            Text = text;
            Position = position;
            Length = length;
        }

        /// <summary>
        /// Resets the state of reader.
        /// </summary>
        public void Reset()
        {
            m_builder.Clear();

            Text = string.Empty;
            Position = 0;
            Length = 0;
        }

        /// <summary>
        /// Peeks current character without changing position.
        /// </summary>
        public char Peek()
        {
            return Text[Position];
        }

        /// <summary>
        /// Reads current character and move position to next character.
        /// </summary>
        public char Read()
        {
            return Text[Position++];
        }

        /// <summary>
        /// Reads and move position with specified length of characters.
        /// </summary>
        /// <param name="length">The length to read.</param>
        public string Read(int length)
        {
            for (int i = 0; i < length; i++)
            {
                m_builder.Append(Read());
            }

            string result = m_builder.ToString();

            m_builder.Clear();
            
            return result;
        }

        /// <summary>
        /// Reads and move position until meet specified character.
        /// </summary>
        /// <param name="ch">The character to stop read.</param>
        public string ReadUntil(char ch)
        {
            while (CanRead() && Peek() != ch)
            {
                m_builder.Append(Read());
            }

            string result = m_builder.ToString();

            m_builder.Clear();

            return result;
        }

        /// <summary>
        /// Reads and move position until specified predicate returns true.
        /// </summary>
        /// <param name="predicate">The predicate to check character for stop.</param>
        public string ReadUntil(Predicate<char> predicate)
        {
            while (CanRead() && !predicate(Peek()))
            {
                m_builder.Append(Read());
            }

            string result = m_builder.ToString();

            m_builder.Clear();

            return result;
        }

        /// <summary>
        /// Determines whether this reader can peek or read next character.
        /// </summary>
        public bool CanRead()
        {
            return Position < Length;
        }

        /// <summary>
        /// Move position skipping white spaces.
        /// </summary>
        public void SkipWhiteSpace()
        {
            while (CanRead() && char.IsWhiteSpace(Peek()))
            {
                Read();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return CanRead() ? $"{Position}: '{Peek()}'" : $"{Position}";
        }
    }
}