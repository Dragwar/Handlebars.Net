﻿using HandlebarsDotNet.StringUtils;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace HandlebarsDotNet
{
    /// <summary>
    /// <inheritdoc />
    /// Produces <c>HTML</c> safe output.
    /// <para>
    /// This will encode non-ascii characters.
    /// this will not encode '=', '`' or ''' (single quote).
    /// </para>
    /// </summary>
    public class HtmlEncoderLegacy : ITextEncoder
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Encode(StringBuilder text, TextWriter target)
        {
            if (text == null || text.Length == 0) return;

            EncodeImpl(new StringBuilderEnumerator(text), target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Encode(string text, TextWriter target)
        {
            if (string.IsNullOrEmpty(text)) return;

            EncodeImpl(new StringEnumerator(text), target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Encode<T>(T text, TextWriter target) where T : IEnumerator<char>
        {
            if (text == null) return;

            EncodeImpl(text, target);
        }

        private static void EncodeImpl<T>(T text, TextWriter target) where T : IEnumerator<char>
        {
            while (text.MoveNext())
            {
                var value = text.Current;
                switch (value)
                {
                    case '"':
                        target.Write("&quot;");
                        break;
                    case '&':
                        target.Write("&amp;");
                        break;
                    case '<':
                        target.Write("&lt;");
                        break;
                    case '>':
                        target.Write("&gt;");
                        break;

                    default:
                        if (value > 159)
                        {
                            target.Write("&#");
                            target.Write((int)value);
                            target.Write(";");
                        }
                        else target.Write(value);
                        break;
                }
            }
        }
    }
}
