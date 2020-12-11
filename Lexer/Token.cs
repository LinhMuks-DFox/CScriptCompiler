namespace MuxLib.CScript.Compiler.Lexer
{
    public class Token
    {
        private readonly string _value;
        private readonly int? _line, _count;

        private readonly TokenType _type;

        public Token(TokenType type, int? line, int? count, string value) =>
            (_type, _line, _count, _value) = (type, line, count - value.Length, value);

        public Token(TokenType type, string value)
            : this(type, null, null, value) { }

        public override string ToString() =>
            // $"Token obj: type: {_type}, value: '{_value}', " +
            // $"line: {_line}, count: {_count}";
            $"({_type}, '{_value}', {_line}, {_count})";

        public bool IsScalar() =>
            _type == TokenType.Boolean || _type == TokenType.Float ||
            _type == TokenType.Integer || _type == TokenType.String;

        public bool IsIdentifier() =>
            _type == TokenType.Identifier;

        public TokenType Type { get => _type; }
        public string Value { get => _value; }

        public int? Line { get => _line; }

        public int? Count { get => _count; }


    }
}