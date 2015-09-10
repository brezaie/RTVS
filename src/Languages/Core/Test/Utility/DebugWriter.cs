﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Microsoft.Languages.Core.Classification;
using Microsoft.Languages.Core.Text;
using Microsoft.Languages.Core.Tokens;

namespace Microsoft.Languages.Core.Test.Utility
{
    [ExcludeFromCodeCoverage]
    public class DebugWriter
    {
        public static string WriteTokens<Token, TokenType>(IReadOnlyTextRangeCollection<Token> tokens) where Token : ITextRange
        {
            var sb = new StringBuilder();

            foreach (Token t in tokens)
            {
                if (t is ICompositeToken)
                {
                    WriteCompositeToken(t as ICompositeToken, sb);
                }
                else
                {
                    WriteToken<Token, TokenType>(t, sb);
                }
            }

            string s = sb.ToString();
            return s;
        }

        private static void WriteToken<Token, TokenType>(Token t, StringBuilder sb)
        {
            IToken<TokenType> token = (IToken<TokenType>)t;
            string enumName = Enum.GetName(typeof(TokenType), token.TokenType);

            int spaceCount = 20 - enumName.Length;
            var sbSpaces = new StringBuilder(spaceCount);
            sbSpaces.Append(' ', spaceCount);

            sb.AppendFormat(CultureInfo.InvariantCulture, "{0}{1} : {2} - {3}\t({4})\r\n", enumName, sbSpaces.ToString(), token.Start, token.End, token.Length);
        }

        private static void WriteCompositeToken(ICompositeToken composite, StringBuilder sb)
        {
            IClassificationNameProvider nameProvider = composite.ClassificationNameProvider;

            foreach (var token in composite.TokenList)
            {
                ITextRange range;
                string classificationName = nameProvider.GetClassificationName(token, out range);

                sb.AppendFormat(CultureInfo.InvariantCulture, "Composite: {0} : {1} - {2}\t({3})\r\n", classificationName, range.Start, range.End, range.Length);
            }
        }
    }
}
