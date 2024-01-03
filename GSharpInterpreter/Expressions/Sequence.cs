using System;
using System.Collections;
using System.Collections.Generic;

namespace GSharpInterpreter
{
    public abstract class Sequence: Expression
    {
    }
    /// <summary>
    /// Represents a finite sequence of expressions of the same type.
    /// </summary>
    public class FiniteSequence : Sequence, IEnumerable<Expression>
    {
        public List<Expression> Elements { get; private set; }
        public FiniteSequence(List<Expression> elements)
        {
            Elements = elements;
        }
        public int Count { get { return Elements.Count; } }
        
        public List<Expression> GetElements()
        {
            return new List<Expression>(Elements);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        public IEnumerator<Expression> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }
    }
    /// <summary>
    /// Represents an infinite sequence of expressions of integers.
    /// </summary>
    public class InfiniteSequence : Sequence, IEnumerable<double>
    {
        public double Start { get; private set; }
        public InfiniteSequence(double start)
        {
            Start = start;
        }
        public IEnumerator<double> GetEnumerator()
        {
            double n = Start;
            while (true)
            {
                yield return n;
                n++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            double n = Start;
            while (true)
            {
                yield return n;
                n++;
            }
        }
    }
    /// <summary>
    /// Represents a sequence of integers in a range.
    /// </summary>
    public class RangeSequence : Sequence, IEnumerable<double>
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
        public IEnumerator<double> GetEnumerator()
        {
            for (double i = Start; i <= End; i++)
            {
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (double i = Start; i <= End; i++)
            {
                yield return i;
            }
        }
    }
}
