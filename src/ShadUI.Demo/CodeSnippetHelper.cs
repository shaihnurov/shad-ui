using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ShadUI.Demo;

public static class CodeSnippetHelper
{
    /// <summary>
    ///     Extracts lines between two line numbers (inclusive, 1-based).
    /// </summary>
    public static string ExtractByLineRange(this string filePath, int startLine, int endLine)
    {
        try
        {
            var lines = File.ReadAllLines(filePath);
            return string.Join(Environment.NewLine,
                lines.Skip(startLine - 1).Take(endLine - startLine + 1));
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static string CleanIndentation(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var minIndent = int.MaxValue;
        var lines = input.Replace("\r\n", "\n").Split('\n');
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var indent = 0;
            while (indent < line.Length && char.IsWhiteSpace(line[indent]))
            {
                indent++;
            }

            if (indent < line.Length && indent < minIndent)
            {
                minIndent = indent;
            }
        }

        if (minIndent == int.MaxValue)
        {
            minIndent = 0;
        }

        var stringBuilder = new StringBuilder(input.Length);
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (i > 0)
            {
                stringBuilder.AppendLine();
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                stringBuilder.Append(line);
            }
            else
            {
                stringBuilder.Append(line.Length >= minIndent ? line[minIndent..] : line);
            }
        }

        return stringBuilder.ToString();
    }
}