using System;
using System.Collections.Generic;
using System.Text;

namespace WorldMachineLoader.Utils
{
    public static class TextUtils
    {
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