using System.IO;
using System.Collections.Generic;

namespace MuxLib.CScript.Compiler.Lexer
{
    class PushBackStreamReader : StreamReader
    {

        private Stack<char> _back_stack;
        public PushBackStreamReader(Stream strm)
            : base(strm)
        {
            _back_stack = new Stack<char>();
        }

        public PushBackStreamReader(string path)
            : base(path)
        {
            _back_stack = new Stack<char>();
        }

        public override int Read()
        {
            if (_back_stack.Count > 0)
            {
                return _back_stack.Pop();
            }
            else
                return base.Read();
        }

        public void PushBack(char ch)  // char, don't allow Pushback(-1)
        {
            _back_stack.Push(ch);
        }
    }
}
