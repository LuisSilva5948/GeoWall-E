using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    public abstract class Statement
    {
    }
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; }
        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }
    }
    public class AssignmentStatement : Statement
    {
        public Token Identifier { get; }
        public Expression Value { get; }
        public AssignmentStatement(Token identifier, Expression value)
        {
            Identifier = identifier;
            Value = value;
        }
    }
    public class IfStatement : Statement
    {
        public Expression Condition { get; }
        public ExpressionStatement ThenBranch { get; }
        public ExpressionStatement ElseBranch { get; }

        public IfStatement(Expression condition, ExpressionStatement thenBranch, ExpressionStatement elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }
    }
    public class LetStatement : Statement
    {
        public List<Statement> Instructions { get; }
        public Expression Body { get; }

        public LetStatement(List<Statement> instructions, Expression body)
        {
            Instructions = instructions;
            Body = body;
        }
    }
}
