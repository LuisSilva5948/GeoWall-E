using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    /// <summary>
    /// Represents a parser that analyzes the tokens produced by the lexer and constructs an abstract syntax tree (AST) for the source code.
    /// </summary>
    public class Parser
    {
        private readonly List<Token> Tokens;    // The list of tokens produced by the lexer
        private int CurrentPosition;            // The current position in the token list
        private bool IsFunctionDeclaration;     // True if the parser is parsing a function declaration (used for error handling)

        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
            CurrentPosition = 0;
            IsFunctionDeclaration = false;
        }
        /// <summary>
        /// Parses the tokens and constructs an abstract syntax tree (AST).
        /// </summary>
        /// <returns>The abstract syntax tree.</returns>
        public List<Expression> Parse()
        {
            List<Expression> AST = new List<Expression>();
            try
            {
                while (!IsAtEnd())
                {
                    IsFunctionDeclaration = false;
                    Expression instruction = Expression();
                    Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
                    AST.Add(instruction);
                }
                return AST;
            }
            catch(Exception e)
            {
                if (e is Error error)
                    throw error;
                else
                throw new Error(ErrorType.SYNTAX, "Invalid Syntax.");
            }
        }
        /// <summary>
        /// Parses the global expression.
        /// </summary>
        private Expression Expression()
        {
            if (Match(TokenType.IDENTIFIER))
                if (Peek().Type == TokenType.COMMA)
                    return MultipleAssignments(Previous());
            return Logical();
        }

        private Expression MultipleAssignments(Token token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a logical expression (&, |).
        /// </summary>
        private Expression Logical()
        {
            Expression expression = Equality();
            while (Match(TokenType.AND, TokenType.OR))
            {
                Token Operator = Previous();
                Expression right = Equality();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses an equality expression (==, !=).
        /// </summary>
        private Expression Equality()
        {
            Expression expression = Comparison();
            while (Match(TokenType.EQUAL, TokenType.NOT_EQUAL))
            {
                Token Operator = Previous();
                Expression right = Comparison();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a comparison expression (>=, >, <, <=).
        /// </summary>
        private Expression Comparison()
        {
            Expression expression = Term();
            while (Match(TokenType.GREATER_EQUAL, TokenType.GREATER, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token Operator = Previous();
                Expression right = Term();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a term expression (+, -).
        /// </summary>
        private Expression Term()
        {
            Expression expression = Factor();
            while (Match(TokenType.ADDITION, TokenType.SUBSTRACTION))
            {
                Token Operator = Previous();
                Expression right = Factor();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a factor expression (*, /, %).
        /// </summary>
        private Expression Factor()
        {
            Expression expression = Power();
            while (Match(TokenType.MULTIPLICATION, TokenType.DIVISION, TokenType.MODULO))
            {
                Token Operator = Previous();
                Expression right = Power();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a power expression (^).
        /// </summary>
        private Expression Power()
        {
            Expression expression = Unary();
            if (Match(TokenType.POWER))
            {
                Token Operator = Previous();
                Expression right = Unary();
                return new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a unary expression (!, -).
        /// </summary>
        private Expression Unary()
        {
            if (Match(TokenType.NOT, TokenType.SUBSTRACTION))
            {
                Token Operator = Previous();
                Expression right = Unary();
                return new UnaryExpression(Operator, right);
            }
            return Literal();
        }
        /// <summary>
        /// Parses a literal expression (number, string, boolean, group).
        /// </summary>
        private Expression Literal()
        {
            if (Match(TokenType.BOOLEAN, TokenType.NUMBER, TokenType.STRING))
            {
                return new LiteralExpression(Previous().Literal);
            }
            if (Match(TokenType.LEFT_PAREN))
            {
                Expression expression = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");
                return new GroupingExpression(expression);
            }
            if (Match(TokenType.LEFT_BRACE))
            {
                return ParseSequence();
            }
            if (Match(TokenType.IDENTIFIER))
            {
                Token id = Previous();
                if (Match(TokenType.LEFT_PAREN))
                    return FunctionCall(id.Lexeme);
                if (Match(TokenType.ASSIGN))
                    return new AssignExpression(id, Expression());
                return new VariableExpression(id);
            }
            else if (Match(TokenType.IF))
                return IfElseStatement();
            else if (Match(TokenType.LET))
                return LetInExpression();
            throw new Error(ErrorType.SYNTAX, $"Expected expression after '{Previous().Lexeme}'.");
        }

        /// <summary>
        /// Parses an if-else statement.
        /// </summary>
        public Expression IfElseStatement()
        {
            Expression condition = Expression();
            Consume(TokenType.THEN, "Expected 'then' after 'if-else' condition.");
            Expression thenBranch = Expression();
            Consume(TokenType.ELSE, "Expected 'else' at 'if-else' expression.");
            Expression elseBranch = Expression();
            return new IfElseStatement(condition, thenBranch, elseBranch);
        }
        /// <summary>
        /// Parses a let-in expression.
        /// </summary>
        public Expression LetInExpression()
        {
            // Parse variable assignments
            List<AssignExpression> assignments = new List<AssignExpression>();
            do
            {
                Token id = Consume(TokenType.IDENTIFIER, "Expected a variable name in a 'let-in' expression.");
                Consume(TokenType.ASSIGN, $"Expected '=' when initializing variable '{id.Lexeme}'.");
                try
                {
                    Expression value = Expression();
                    assignments.Add(new AssignExpression(id, value));
                }
                catch (Error)
                {
                    throw new Error(ErrorType.SYNTAX, $"Expected value of '{id.Lexeme}' after '='.");
                }
                Consume(TokenType.SEMICOLON, $"Expected ';' after assignment of '{id}'.");
            }
            while (Peek().Type == TokenType.IDENTIFIER);

            Consume(TokenType.IN, "Expected 'in' at 'let-in' expression.");
            Expression body = Expression();
            return new LetInExpression(assignments, body);
        }
        /// <summary>
        /// Parses a function call or declaration.
        /// </summary>
        public Expression FunctionCall(string id)
        {
            // Parse function arguments
            List<Expression> arguments = new List<Expression>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    Expression argument = Expression();
                    arguments.Add(argument);
                }
                while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, $"Expected ')' after '{id}' arguments.");
            // Check if function is being declared or called
            if (Match(TokenType.ASSIGN))
                return FunctionDeclaration(id, arguments);
            return new FunctionCall(id, arguments);
        }
        /// <summary>
        /// Parses a function declaration.
        /// </summary>
        public Expression FunctionDeclaration(string id, List<Expression> arguments)
        {
            // Set flag to true to not parse another FunctionDeclaration inside this one 
            IsFunctionDeclaration = true;
            // Check if function already exists
            if (StandardLibrary.DeclaredFunctions.ContainsKey(id))
                throw new Error(ErrorType.SYNTAX, $"Function '{id}' already exists and can't be redefined.");
            // Add function to declared functions temporarily
            StandardLibrary.DeclaredFunctions[id] = null;
            List<VariableExpression> parameters = new List<VariableExpression>();
            // Check if arguments are parameters and are not repeated
            foreach (Expression argument in arguments)
            {
                if (argument is not VariableExpression parameter)
                    throw new Error(ErrorType.SYNTAX, "Expected valid variable name as parameter in function declaration.");
                if (parameters.Contains(parameter))
                {
                    throw new Error(ErrorType.SYNTAX, $"Parameter name '{parameter.ID}' cannot be used more than once.");
                }
                parameters.Add(parameter);
            }
            try
            {
                Expression body = Expression();
                FunctionDeclaration function = new FunctionDeclaration(id, parameters, body);
                // Add function to declared functions permanently
                StandardLibrary.AddFunction(function);
                return function;
            }
            catch (Error)
            {
                // Remove invalid function from declared functions
                StandardLibrary.DeclaredFunctions.Remove(id);
                throw new Error(ErrorType.SYNTAX, $"Invalid declaration of function '{id}'.");
            }
        }
        private Expression ParseSequence()
        {
            throw new NotImplementedException();
            if (Match(TokenType.RIGHT_BRACE)) return Expression();
        }


        #region Helper Methods
        /// <summary>
        /// Matches the given token types and advances the current position if a match is found.
        /// </summary>
        /// <param name="types">The token types to match.</param>
        /// <returns>True if a match is found; otherwise, false.</returns>
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the current token has the specified type.
        /// </summary>
        /// <param name="type">The token type to check.</param>
        /// <returns>True if the current token has the specified type; otherwise, false.</returns>
        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        /// <summary>
        /// Checks if the current position is at the end of the token list.
        /// </summary>
        /// <returns>True if the current position is at the end; otherwise, false.</returns>
        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        /// <summary>
        /// Advances the current position to the next token and returns the previous token.
        /// </summary>
        /// <returns>The previous token.</returns>
        private Token Advance()
        {
            if (!IsAtEnd()) CurrentPosition++;
            return Previous();
        }

        /// <summary>
        /// Returns the token at the current position without advancing the position.
        /// </summary>
        /// <returns>The token at the current position.</returns>
        private Token Peek()
        {
            return Tokens[CurrentPosition];
        }

        /// <summary>
        /// Returns the previous token.
        /// </summary>
        /// <returns>The previous token.</returns>
        private Token Previous()
        {
            return Tokens[CurrentPosition - 1];
        }

        /// <summary>
        /// Consumes the current token if it has the specified type; otherwise, throws a syntax error.
        /// </summary>
        /// <param name="type">The expected token type.</param>
        /// <param name="message">The error message to throw.</param>
        /// <returns>The consumed token.</returns>
        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
                return Advance();

            throw new Error(ErrorType.SYNTAX, message);
        }
        #endregion
    }
}