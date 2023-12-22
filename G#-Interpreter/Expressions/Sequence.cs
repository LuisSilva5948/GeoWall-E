using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GSharpInterpreter
{
    public abstract class Sequence: Expression
    {
    }
    /// <summary>
    /// Represents a finite sequence of expressions of the same type.
    /// </summary>
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
    /// <summary>
    /// Represents an infinite sequence of expressions of integers.
    /// </summary>
    public class InfiniteSequence : Sequence
    {
        public double Start { get; private set; }
        public InfiniteSequence(double start)
        {
            Start = start;
        }
        public double GetElement()
        {
            return Start++;
        }
    }
    /// <summary>
    /// Represents a sequence of integers in a range.
    /// </summary>
    public class RangeSequence : Sequence
    {
        public double Start { get; private set; }
        public double End { get; }
        public int Count
        {
            get
            {
                if (Start - End >= 0) return (int)Start - (int)End + 1;
                else return 0;
            }
        }
        public RangeSequence(double start, double end)
        {
            Start = start;
            End = end;
        }
        public object GetElement()
        {
            if (Count > 0)
            {
                return Start++;
            }
            else return new Undefined();
        }
    }
}
