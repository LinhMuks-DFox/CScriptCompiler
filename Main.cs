using System;
using System.IO;

namespace MuxLib.CScript.Compiler.Lexer
{
    public class MainProgram
    {
        public static void Main()
        {
            const string path = @"D:\MuxLib\Lexer\csp_test\hello_world.csp";
            const string path_LIR = @"D:\MuxLib\Lexer\csp_test\hello_world.csp.LIR";
            Lexer test = new Lexer(true, path);
            foreach (Token i in test.Tokens)
                Console.WriteLine(i);
            using StreamWriter sw = new StreamWriter(path_LIR);
            foreach (Token i in test.Tokens)
                sw.WriteLine(i);
        }
    }
}
