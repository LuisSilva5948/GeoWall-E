using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    public abstract class Sequence { }

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
    public class FiniteSequence : Expression
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
            else return new UndefinedExpression();
        }
    }
    public class PointSequence : Sequence
    {
        public List<Expression> Elements { get; }
        public PointSequence(List<Expression> elements)
        {
            Elements = elements;
        }
    }
}
