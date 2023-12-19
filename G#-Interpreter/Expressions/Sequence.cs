using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    public abstract class Sequence: Expression
    {
    }

    //three types of sequences: infinite, finite(range) and values(array)
    /*public class ValuesSequence : Sequence
    {
        public List<Expression> Values { get; private set; }
        public TypeSequence SequenceType = TypeSequence.Values;
        public override TypeSequence GetSequenceType => SequenceType;

        public ValuesSecquence(List<Expression> secquenceValues)
        {
            this.secquenceValues = secquenceValues;
            expressionLine = Parser.GetLine;
        }

    }*/
    public class FiniteSequence : Sequence
    {
        public List<Expression> Elements { get; }
        public FiniteSequence(List<Expression> elements)
        {
            Elements = elements;
        }
        public int Count { get { return Elements.Count; } }
        public Expression GetElement()
        {
            if (Count > 0)
            {
                Expression next = Elements[0];
                Elements.RemoveAt(0);
                return next;
            }
            else return new Undefined();
        }
    }
    public class FigureSequence : Sequence
    {
        public List<Expression> Elements { get; }
        public FigureSequence(List<Expression> elements)
        {
            Elements = elements;
        }
    }
    public class InfiniteSequence : Sequence
    {
        public Expression Expression { get; }
        public InfiniteSequence(Expression expression)
        {
            Expression = expression;
        }
    }
    public class RangeSequence : Sequence
    {
        public Expression Start { get; }
        public Expression End { get; }
        public Expression Step { get; }
        public RangeSequence(Expression start, Expression end, Expression step)
        {
            Start = start;
            End = end;
            Step = step;
        }
    }
}
