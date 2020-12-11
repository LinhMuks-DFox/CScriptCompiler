using System;

namespace MuxLib.CScript.Compiler.Lexer
{
    public enum TokenType
    {
        #region scalar
        String,
        Integer,
        Float,
        Boolean,
        #endregion // scalar

        #region Syntax
        Keyword,
        Operator,
        Bracket,
        Identifier,
        Delimiter
        #endregion //Syntax
    }

    public enum OperatorsType
    {
        #region arithmetic
        Add,
        Minus,
        Multiply,
        Div,
        #endregion // arithmetic

        #region Bool Operation
        Equl,
        Bigger,
        Smaller,
        EqulBigger,
        EqulSmaller,
        #endregion Bool Operation

        #region Syntax
        Assign,
        #endregion //Syntax
    }
}