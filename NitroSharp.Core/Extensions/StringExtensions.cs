using System.Runtime.CompilerServices;

namespace NitroSharp.Core.Extensions;

public static class StringExtensions
{
    extension(string str)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringBefore(char delimiter)
        {
            var index = str.IndexOf(delimiter);
            return index == -1 ? str : str[..index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringBefore(string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
        {
            var index = str.IndexOf(delimiter, comparisonType);
            return index == -1 ? str : str[..index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringAfter(char delimiter)
        {
            var index = str.IndexOf(delimiter);
            return index == -1 ? str : str[(index + 1)..];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringAfter(string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
        {
            var index = str.IndexOf(delimiter, comparisonType);
            return index == -1 ? str : str.Substring(index + delimiter.Length, str.Length - index - delimiter.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringBeforeLast(char delimiter)
        {
            var index = str.LastIndexOf(delimiter);
            return index == -1 ? str : str[..index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringBeforeWithLast(char delimiter)
        {
            var index = str.LastIndexOf(delimiter);
            return index == -1 ? str : str[..(index + 1)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringBeforeLast(string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
        {
            var index = str.LastIndexOf(delimiter, comparisonType);
            return index == -1 ? str : str[..index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringAfterLast(char delimiter)
        {
            var index = str.LastIndexOf(delimiter);
            return index == -1 ? str : str.Substring(index + 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringAfterWithLast(char delimiter)
        {
            var index = str.LastIndexOf(delimiter);
            return index == -1 ? str : str[index..];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string SubstringAfterLast(string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
        {
            var index = str.LastIndexOf(delimiter, comparisonType);
            return index == -1 ? str : str.Substring(index + delimiter.Length, str.Length - index - delimiter.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(string value, StringComparison comparisonType) =>
            str.Contains(value, comparisonType);
    }
}