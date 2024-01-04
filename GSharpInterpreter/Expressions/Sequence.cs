using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GSharpInterpreter
{
    public abstract class Sequence: Expression, IEnumerable<Expression>
    {
        public GSharpType ElementType { get; }
        public Sequence? Concatenated { get; set; }
        public abstract void Concatenate(Sequence sequence);
        public abstract double Count { get; }
        public abstract IEnumerator<Expression> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Sequence operator +(Sequence s1, Sequence s2)
        {
            if (s1 is InfiniteSequence infiniteSeq)
                return infiniteSeq;
            if (s1 is FiniteSequence finiteSeq)
            {
                if (finiteSeq.Count == 0) return s2;
            }
            if (s1 is RangeSequence rangeSeq)
            {
                if (rangeSeq.Count == 0) return s2;
            }
            if (s2 is FiniteSequence finite)
            {
                if (finite.Count == 0) return s1;
            }
            s1.Concatenate(s2);
            return s1;
        }
        public static Sequence operator +(Sequence seq, Undefined undefined)
        {
            return seq;
        }
        public static Undefined operator +(Undefined undefined, Sequence seq)
        {
            return undefined;
        }
    }
    /// <summary>
    /// Represents a finite sequence of expressions of the same type.
    /// </summary>
    public class FiniteSequence : Sequence, IEnumerable<Expression>
    {
        public GSharpType Type => GSharpType.SEQUENCE;
        public GSharpType ElementType => GetElementType();
        public GSharpType GetElementType()
        {
            if (Elements.Count > 0)
            {
                if (Elements[0] is Measure) return GSharpType.MEASURE;
                else if (Elements[0] is Point) return GSharpType.POINT;
                else if (Elements[0] is Line) return GSharpType.LINE;
                else if (Elements[0] is Ray) return GSharpType.RAY;
                else if (Elements[0] is Segment) return GSharpType.SEGMENT;
                else if (Elements[0] is Circle) return GSharpType.CIRCLE;
                else if (Elements[0] is Arc) return GSharpType.ARC;
                else if (Elements[0] is GSharpString) return GSharpType.STRING;
                else if (Elements[0] is GSharpNumber) return GSharpType.NUMBER;
                else return GSharpType.UNDEFINED;
            }
            else return GSharpType.UNDEFINED;
        }
        public List<Expression> Elements { get; private set; }
        public FiniteSequence(List<Expression> elements)
        {
            Elements = elements;
        }
        public override double Count
        {
            get
            {
                if (Concatenated != null)
                {
                    return Elements.Count + Concatenated.Count;
                }
                else return Elements.Count;
            }
        }
        
        public List<Expression> GetElements()
        {
            return new List<Expression>(Elements);
        }
        public override IEnumerator<Expression> GetEnumerator()
        {
            foreach (Expression element in Elements)
            {
                yield return element;
            }

            if (Concatenated != null)
            {
                var enumerator = Concatenated.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        
        public override void Concatenate(Sequence sequence)
        {
            if (Count > 0)
            {
                if (ElementType == sequence.ElementType)
                {
                    if (Concatenated == null)
                        Concatenated = sequence;
                    else
                        Concatenated.Concatenate(sequence);
                }
                else throw new GSharpError(ErrorType.SEMANTIC, $"Cannot concatenate a sequence of type {sequence.ElementType} to a sequence of type {ElementType}.");
            }
            else
            {
                if (Concatenated == null)
                {
                    Concatenated = sequence;
                }
                else
                {
                    Concatenated.Concatenate(sequence);
                }
            }
        }
        public override string ToString()
        {
            string result = "";
            if (Elements.Count == 0) return "{ }";
            result = "{ ";
            foreach (Expression element in Elements)
            {
                Console.WriteLine(element);
                result += element.ToString() + ", ";
            }
            result = result.Remove(result.Length - 2);
            result += " }";
            if (Concatenated != null)
            {
                result += " + " + Concatenated.ToString();
            }
            return result;
        }
    }
    /// <summary>
    /// Represents an infinite sequence of expressions of integers.
    /// </summary>
    public class InfiniteSequence : Sequence, IEnumerable<Expression>
    {
        public GSharpType Type => GSharpType.SEQUENCE;
        public GSharpType ElementType => GSharpType.NUMBER;
        public GSharpNumber Start { get; private set; }

        public InfiniteSequence(GSharpNumber start)
        {
            Start = start;
        }
        public override double Count => double.PositiveInfinity;
        public override IEnumerator<Expression> GetEnumerator()
        {
            GSharpNumber n = Start;
            while (true)
            {
                yield return n;
                n = new GSharpNumber(n.Value + 1);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return $"{{ {Start}... }}";
        }

        public override void Concatenate(Sequence sequence)
        {
            return;
        }
    }

    public class RangeSequence : Sequence, IEnumerable<Expression>
    {
        public GSharpType Type => GSharpType.SEQUENCE;
        public GSharpType ElementType => GSharpType.NUMBER;
        public GSharpNumber Start { get; }
        public GSharpNumber End { get; }
        public override double Count
        {
            get
            {
                if (Concatenated != null)
                {
                    if (End.Value - Start.Value >= 0) return (End.Value - Start.Value) + 1 + Concatenated.Count;
                    else return Concatenated.Count;
                }
                if (End.Value - Start.Value >= 0) return (End.Value - Start.Value) + 1;
                else return 0;
            }
        }

        public RangeSequence(GSharpNumber start, GSharpNumber end)
        {
            Start = start;
            End = end;
        }

        public override IEnumerator<Expression> GetEnumerator()
        {
            if (Start.Value <= End.Value)
            {
                for (double i = Start.Value; i <= End.Value; i++)
                {
                    yield return new GSharpNumber(i);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            string result = "";
            result = $"{{ {Start}...{End} }}";
            if (Concatenated != null)
            {
                result += " + " + Concatenated.ToString();
            }
            return result;
        }

        public override void Concatenate(Sequence sequence)
        {
            if (Count > 0)
            {
                if (ElementType == sequence.ElementType)
                {
                    if (Concatenated == null)
                    {
                        Concatenated = sequence;
                    }
                    else
                    {
                        Concatenated.Concatenate(sequence);
                    }
                }
                else throw new GSharpError(ErrorType.SEMANTIC, $"Cannot concatenate a sequence of type {sequence.ElementType} to a sequence of type {ElementType}.");
            }
            else
            {
                if (Concatenated == null)
                {
                    Concatenated = sequence;
                }
                else
                {
                    Concatenated.Concatenate(sequence);
                }
            }
        }
    }
}
