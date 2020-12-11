using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace MuxLib.CScript.Compiler.Lexer
{
    /// <summary>
    /// 通用词法分析器
    /// </summary>
    public class Lexer
    {
        private readonly List<Token> _tokens = null;
        public Lexer() =>
            _tokens = new List<Token>();
        /// <summary>
        /// 构造时直接设置分析文件的路径并且直接分析
        /// </summary>
        /// <param name="lexup"></param>
        /// <param name="path"></param>
        public Lexer(bool lexup, string path)
        {
            SourcePath = path;
            _tokens = new List<Token>();
            if (lexup)
                LexUp();
        }
        /// <summary>
        /// 源代码路径
        /// </summary>
        public string SourcePath { get; set; }
        public List<Token> Tokens { get => _tokens; }
        /// <summary>
        /// 核心词法分析
        /// </summary>
        public void LexUp()
        {
            if (!File.Exists(SourcePath))
                throw new IOException($"{SourcePath} do not exist.");
            using PushBackStreamReader fi = new PushBackStreamReader(SourcePath);
            int line = 0, count = 0;
            string buffer = "";
            while (fi.Peek() >= 0)
            {
                char c = (char)fi.Read();
                if (c == 0)
                    continue;

                if (c == '\n' || c == '\r')
                {
                    if (c == '\n')
                    {
                        line++;
                        count = 0;
                    }
                    FlushUpBuffer(ref buffer, line, count);
                    continue;
                }
                count++;
                if (c == '#')
                {
                    FlushUpBuffer(ref buffer, line, count);
                    fi.PushBack(c);
                    SkipDoc(fi);
                    continue;
                }
                if (c == '/' && (char)fi.Peek() == '*')
                {
                    FlushUpBuffer(ref buffer, line, count);
                    fi.PushBack(c);
                    SkipDoc(fi, 1);
                    continue;
                }

                if (IsBracket(c))
                {

                    FlushUpBuffer(ref buffer, line, count);
                    Tokens.Add(new Token(TokenType.Bracket, line, count, "" + c));
                    continue;
                }
                if (c == '"' || c == '\'')
                {
                    FlushUpBuffer(ref buffer, line, count);
                    fi.PushBack(c);
                    Tokens.Add(new Token(TokenType.String, line, count, MakeString(fi)));
                    continue;
                }
                if (IsOperator("" + c))
                {

                    if (IsOperator("" + (char)fi.Peek()))
                    {

                        FlushUpBuffer(ref buffer, line, count);
                        Tokens.Add(new Token(TokenType.Operator, line, count, ""
                            + c + (char)fi.Peek()));
                        fi.Read();
                        continue;
                    }
                    FlushUpBuffer(ref buffer, line, count);
                    Tokens.Add(new Token(TokenType.Operator, line, count, "" + c));
                    continue;
                }
                if (c == ' ' || c == '\t')
                {

                    FlushUpBuffer(ref buffer, line, count);
                    continue;
                }
                if (c == ';')
                {

                    FlushUpBuffer(ref buffer, line, count);
                    Tokens.Add(new Token(TokenType.Delimiter, line, count, "" + c));
                    continue;
                }
                buffer += c;
            }
        }
        /*
        private bool IsLetter(string c) =>
            return Regex.IsMatch(c, @"^[a-zA-Z_]$");
        */
        private void SkipDoc(PushBackStreamReader sr, int mode = 0)
        {

            while (sr.Peek() >= 0)
            {
                if (mode == 0)
                {
                    char c = (char)sr.Read();
                    if (c == '#')
                        continue;
                    if (c == '\n' || c == '\r')
                        return;
                }
                if (mode == 1)
                {
                    char c = (char)sr.Read();
                    if (c == '*' && (char)sr.Peek() == '/')
                    {
                        sr.Read(); // skip the / of/**/
                        return;
                    }
                }
            }


        }
        private string MakeString(PushBackStreamReader sr)
        {
            string s = "";
            int state = 0;
            while (sr.Peek() >= 0)
            {
                char c = (char)sr.Read();
                switch (state)
                {
                    case 0:
                        if (c == '\"')
                            state = 1;
                        else
                            state = 2;
                        s += c;
                        break;
                    case 1:
                        if (c == '"')
                            return s += c;
                        else
                            s += c;
                        break;
                    case 2:
                        if (c == '\'')
                            return s += c;
                        else
                            s += c;
                        break;
                }
            } // end while
            throw new LexicalError("Unexpected error");
        }

        private void FlushUpBuffer(ref string buffer, int line, int count)
        {
            if (buffer == "" || buffer == "\n" || buffer == "\r")
                return;
            Tokens.Add(new Token(MatchType(buffer), line, count, buffer));
            buffer = "";
        }
        public static bool IsBracket(char c)
        {
            char[] brackets = { '{', '}', '(', ')', '[', ']' };
            foreach (char n in brackets)
                if (c == n)
                    return true;
            return false;
        }

        public static bool IsOperator(string c) =>
            Regex.IsMatch(c, @"^[*+\-<>=!&|^%/,:]$", RegexOptions.Compiled) ||
            Regex.IsMatch(c, @"^(..)$", RegexOptions.Compiled);

        public bool IsKeyword(string str)
        {
            string[] _regexes = {
            "^while$", "^byte$", "^global$", "^for$", "^class$",
            "^export$", "^int$", "^bool$", "^if$", "^catch$",
            "^not$", "^new$", "^array$", "^float$", "^delete$",
            "^else$", "^func$", "^finally$", "^string$",
            "^where$", "^var$", "^elif$", "^return$", "^and$", "^is$",
            "^decimal$", "^import$", "^struct$", "^yield$", "^or$", "^try$",
            "^public$", "^private$", "^namespace$", "^using$", "^as$", "^in$" };

            foreach (string pattern in _regexes)
            {
                if (Regex.IsMatch(str, pattern, RegexOptions.Compiled))
                    return true;
            }

            return false;
        }
        public bool IsFloat(string str) =>
            Regex.IsMatch(str, @"^[+-]?[0-9]*(\.)[0-9]*[Ee]?[-+]?[0-9]*$", RegexOptions.Compiled);

        public bool IsInteger(string str) =>
            Regex.IsMatch(str,
                    @"^[+-]?[0-9]*[Ee]?[-+]?[0-9]*$", RegexOptions.Compiled);
        public bool IsBoolean(string str)
        {
            string[] regex = { "^True$", "^true$", "^False$", "^false$", };
            foreach (var i in regex)
                if (Regex.IsMatch(str, i, RegexOptions.Compiled))
                    return true;
            return false;
        }

        public bool IsValidIdentifier(string str) =>
            Regex.IsMatch(str, "^[a-zA-Z_]");
        public TokenType MatchType(string str)
        {
            if (IsInteger(str))
                return TokenType.Integer;
            else if (IsKeyword(str))
                return TokenType.Keyword;
            else if (IsFloat(str))
                return TokenType.Float;
            else if (IsBoolean(str))
                return TokenType.Boolean;
            else return TokenType.Identifier;

        }
    }
}