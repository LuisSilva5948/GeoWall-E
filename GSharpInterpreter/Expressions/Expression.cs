using System;
using System.Collections.Generic;

namespace GSharpInterpreter
{
    /// <summary>
    /// Base class for all types of expressions.
    /// </summary>
    public abstract class Expression
    {
    }
    /// <summary>
    /// Represents a binary expression composed of a left expression, an operator token, and a right expression.
    /// </summary>
    public class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Token Operator { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, Token op, Expression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }

    /// <summary>
    /// Represents a unary expression composed of an operator token and a right expression.
    /// </summary>
    public class UnaryExpression : Expression
    {
        public Token Operator { get; }
        public Expression Right { get; }
        public UnaryExpression(Token op, Expression right)
        {
            Operator = op;
            Right = right;
        }
    }

    /// <summary>
    /// Represents a literal value expression, such as a number or string.
    /// </summary>
    public abstract class LiteralExpression : Expression
    {
    }
    /// <summary>
    /// Represents a number literal expression.
    /// </summary>
    public class GSharpNumber : LiteralExpression, IGSharpObject
    {
        public GSharpType Type => GSharpType.NUMBER;
        public double Value { get; }
        public GSharpNumber(double value)
        {
            Value = value;
        }
        public static GSharpNumber operator +(GSharpNumber n1, GSharpNumber n2)
        {
            return new GSharpNumber(n1.Value + n2.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    /// <summary>
    /// Represents a string literal expression.
    /// </summary>
    public class GSharpString : LiteralExpression, IGSharpObject
    {
        public GSharpType Type => GSharpType.STRING;
        public string Value { get; }
        public GSharpString(string value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
    /// <summary>
    /// Represents a grouping expression that wraps another expression.
    /// </summary>
    public class GroupingExpression : Expression
    {
        public Expression Expression { get; }

        public GroupingExpression(Expression expression)
        {
            Expression = expression;
        }
    }


    /// <summary>
    /// Represents a variable expression identified by a string.
    /// </summary>
    public class ConstantExpression : Expression
    {
        public string ID { get; }

        public ConstantExpression(string id)
        {
            ID = id;
        }
        public override string ToString()
        {
            return ID;
        }
    }

    /// <summary>
    /// Represents an if-else statement with a condition expression, a then branch expression, and an optional else branch expression.
    /// </summary>
    public class Conditional : Expression
    {
        public Expression Condition { get; }
        public Expression ThenBranch { get; }
        public Expression ElseBranch { get; }

        public Conditional(Expression condition, Expression thenBranch, Expression elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }
    }

    /// <summary>
    /// Represents a let-in expression, where a set of instructions are executed before the body expression.
    /// </summary>
    public class LetExpression : Expression
    {
        public List<Expression> Instructions { get; }
        public Expression Body { get; }

        public LetExpression(List<Expression> instructions, Expression body)
        {
            Instructions = instructions;
            Body = body;
        }
    }

    /// <summary>
    /// Represents an assignment expression, where a value is assigned to a variable identified by a string.
    /// </summary>
    public class Assignment : Expression
    {
        public string ID { get; }
        public Expression Value { get; }

        public Assignment(string id, Expression value)
        {
            ID = id;
            Value = value;
        }
    }

    /// <summary>
    /// Represents a match-assignment expression, where elements of a sequence are assigned to a list of variables identified by a list of strings.
    /// </summary>
    public class MultipleAssignment : Expression
    {
        public List<string> IDs { get; }
        public Expression Sequence { get; }

        public MultipleAssignment(List<string> ids, Expression seq)
        {
            IDs = ids;
            Sequence = seq;
        }
    }

    /// <summary>
    /// Represents a function declaration with an identifier, a list of argument variable expressions, and a body expression.
    /// </summary>
    public class Function : Expression
    {
        public string Identifier { get; }
        public List<ConstantExpression> Parameters { get; }
        public Expression Body { get; }

        public Function(string identifier, List<ConstantExpression> parameters, Expression body)
        {
            Identifier = identifier;
            Parameters = parameters;
            Body = body;
        }
    }

    /// <summary>
    /// Represents a call to a function with an identifier and a list of argument expressions.
    /// </summary>
    public class Call : Expression
    {
        public string Identifier { get; }
        public List<Expression> Arguments { get; }

        public Call(string identifier, List<Expression> arguments)
        {
            Identifier = identifier;
            Arguments = arguments;
        }
    }
    /// <summary>
    /// Class that representes undefined or null expressions.
    /// </summary>
    public class Undefined : Expression, IGSharpObject
    {
        public Undefined() { }
        public override string ToString()
        {
            return "Undefined";
        }
        public GSharpType Type => GSharpType.UNDEFINED;
        public static Object operator +(Object obj, Undefined undefined)
        {
            return obj;
        }
        public static Undefined operator +(Undefined undefined, Object obj)
        {
            return undefined;
        }
    }

    /// <summary>
    /// Represents an import statement with a path string.
    /// </summary>
    public class ImportStatement : Expression
    {
        public string Path { get; }
        public ImportStatement(string path)
        {
            Path = path;
        }
    }
    /// <summary>
    /// Represents a draw statement with a figure (as expression) to draw.
    /// </summary>
    public class DrawStatement : Expression
    {
        public Expression Expression { get; }
        public string? Label { get; set; }
        public DrawStatement(Expression expression, string label)
        {
            Expression = expression;
            Label = label;
        }
        public DrawStatement(Expression expression)
        {
            Expression = expression;
        }
    }
    /// <summary>
    /// Represents a print statement with an expression to print.
    /// </summary>
    public class PrintStatement : Expression
    {
        public Expression Expression { get; }
        public string? Label { get; set; }
        public PrintStatement(Expression expression)
        {
            Expression = expression;
        }
        public PrintStatement(Expression expression, string label)
        {
            Expression = expression;
            Label = label;
        }
    }
    /// <summary>
    /// Represents a color statement with a color to set.
    /// </summary>
    public class ColorStatement : Expression
    {
        public GSharpColor Color { get; }
        public ColorStatement(GSharpColor color)
        {
            Color = color;
        }
    }
    /// <summary>
    /// Represents a restore statement that restores the default color.
    /// </summary>
    public class RestoreStatement : Expression
    {
        public RestoreStatement() { }
    }
    public class RandomDeclaration : Expression
    {
        public string Name { get; }
        public GSharpType Type { get; }
        public bool IsSequence { get; }
        public RandomDeclaration(string name, GSharpType type, bool isSequence)
        {
            Name = name;
            Type = type;
            IsSequence = isSequence;
        }
    }
}
