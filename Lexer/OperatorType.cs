using System;
using System.Collections.Generic;
using System.Text;

namespace MuxLib.CScript.Compiler.Lexer
{
    public enum OperatorType
    {
        #region OneChar
        Percent,    // %
        Amper,      // &
        Lpar,       // (
        Rpar,       // )
        Star,       // *
        Plus,       // +
        Comma,      // ,
        Minus,      // -
        Dot,        // .
        Slash,      // /
        Colon,      // :
        Less,       // <
        Equal,      // =
        Greater,    // >
        At,         // @
        Lsqb,       // [
        Rsqb,       // ]
        Circumflex, // ^
        Lbrace,     // {
        Vbar,       // |
        Rbrace,     // }
        Tilde,
        #endregion // OneChar

        #region DoubleChar
        NotEqual,
        PercentEqual,
        AmperEqual,
        DoubleStart,
        StarEqual,
        PlusEqual,
        MinQual,
        Rarrow,
        DoubleSlash,
        SlashEqual,
        ColonEqual,
        LeftShift,
        LessEqual,
        EqEqual,
        GreaterEqual,
        RightShift,
        AtEqual,
        CircumflexEqual,
        VbarEqual,
        #endregion

        #region Three Char

        DoubeleStartQual,
        Qllipsis,
        DoubleSlashEqual,
        LeftShiftEqual,
        RightShiftEqual,

        #endregion
    }
}
