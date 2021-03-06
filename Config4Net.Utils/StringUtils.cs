﻿using System.Text.RegularExpressions;

namespace Config4Net.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// https://stackoverflow.com/questions/323314/best-way-to-convert-pascal-case-to-a-sentence
        /// </summary>
        public static string ToFriendlyString(string st)
        {
            Precondition.ArgumentNotNull(st, nameof(st));
            return Regex.Replace(st, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        }

        public static string ToVariableName(string st)
        {
            Precondition.ArgumentNotNull(st, nameof(st));
            var chars = st.ToCharArray();
            if (chars.Length > 0)
                chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }
    }
}