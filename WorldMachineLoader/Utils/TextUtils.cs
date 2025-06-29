using System;
using System.Collections.Generic;
using System.Text;

namespace WorldMachineLoader.Utils
{
    public static class TextUtils
    {
        /// <summary>
        /// Splits the given <paramref name="text"/> into multiple lines, each of which has a length
        /// not exceeding <paramref name="maxLineLength"/>.
        /// </summary>
        /// <param name="text">The input string to wrap.</param>
        /// <param name="maxLineLength">The maximum allowed length for each line.</param>
        /// <returns>A list of strings, where each string represents one wrapped line.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="text"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxLineLength"/> is less than 1.</exception>
        public static List<string> WrapText(string text, int maxLineLength)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (maxLineLength < 1) throw new ArgumentOutOfRangeException(nameof(maxLineLength));

            var result = new List<string>();
            var words = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            var currentLine = new StringBuilder();

            foreach (var word in words)
            {
                if (currentLine.Length > 0 &&
                    currentLine.Length + 1 + word.Length > maxLineLength)
                {
                    result.Add(currentLine.ToString());
                    currentLine.Clear();
                }

                if (word.Length > maxLineLength)
                {
                    int index = 0;
                    while (index < word.Length)
                    {
                        int chunkSize = Math.Min(maxLineLength, word.Length - index);
                        var chunk = word.Substring(index, chunkSize);
                        if (currentLine.Length > 0)
                        {
                            result.Add(currentLine.ToString());
                            currentLine.Clear();
                        }
                        result.Add(chunk);
                        index += chunkSize;
                    }
                }
                else
                {
                    if (currentLine.Length > 0)
                        currentLine.Append(' ');
                    currentLine.Append(word);
                }
            }

            if (currentLine.Length > 0)
                result.Add(currentLine.ToString());

            return result;
        }
    }
}