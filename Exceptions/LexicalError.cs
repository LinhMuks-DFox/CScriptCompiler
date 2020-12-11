using System;

namespace MuxLib.CScript.Compiler.Lexer
{
    public class LexicalError: Exception
    {
        public LexicalError(string msg):base(msg){}
        public LexicalError() : base(){}
    }
}