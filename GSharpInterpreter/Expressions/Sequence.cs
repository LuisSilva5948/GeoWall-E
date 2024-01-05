using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GSharpInterpreter
{
    public enum SequenceType
    {
        FINITE,
        INFINITE
    }
    public abstract class Sequence: Expression, IEnumerable<Expression>
    {
        public GSharpType Type => GSharpType.SEQUENCE;
        public SequenceType SequenceType => double.IsInfinity(SpecificCount) ? SequenceType.INFINITE : SequenceType.FINITE;
        public GSharpType ElementType { get; }
        public Sequence? Parent { get; private set; }
        public Sequence? Concatenated { get; set; }
        public abstract Sequence GetSequenceTail(double index);
        public Sequence FindSequenceTail(double index)
        {
            if (double.IsInfinity(SpecificCount))
                return GetSequenceTail(index);
            if (index >= 0 && index <= TotalCount)
            {
                if (index <= SpecificCount)
                    return GetSequenceTail(index);
                else if (Concatenated != null)
                    return Concatenated.FindSequenceTail(index - SpecificCount);
                else return new FiniteSequence(new List<Expression>());
            }
            else return new FiniteSequence(new List<Expression>());
        }
        public abstract void Concatenate(Sequence sequence);
        public abstract double TotalCount { get; }
        public abstract double SpecificCount { get; }
        public GSharpType GetElementType()
        {
            if (ElementType != GSharpType.EMPTY)
                return ElementType;
            if (Parent != null)
            {
                GSharpType type = Parent.ElementType;
                if (type == GSharpType.EMPTY)
                {
                    return Parent.GetElementType();
                }
                else return type;
            }
            else return this.ElementType;
        }
        public abstract IEnumerator<Expression> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Sequence operator +(Sequence s1, Sequence s2)
        {
            s1.Concatenate(s2);
            return s1;
        }
    }
    /// <summary>
    /// Represents a finite sequence of expressions of the same type.
    /// </summary>
    public class FiniteSequence : Sequence, IEnumerable<Expression>, IGSharpObject
    {
        public GSharpType Type => GSharpType.SEQUENCE;

        public GSharpType ElementType => elementType;
        private GSharpType elementType = GSharpType.UNDEFINED;

        public List<Expression> Elements { get; private set; }
        public FiniteSequence(List<Expression> elements)
        {
            Elements = elements;
        }
        public FiniteSequence(List<Expression> elements, Sequence concatenated)
        {
            Elements = elements;
            Concatenated = concatenated;
        }
        public FiniteSequence(List<Expression> elements, GSharpType sequenceType)
        {
            Elements = elements;
            elementType = sequenceType;
        }
        public FiniteSequence(List<Expression> elements, Sequence concatenated, GSharpType sequenceType)
        {
            Elements = elements;
            Concatenated = concatenated;
            elementType = sequenceType;
        }
        public override double TotalCount
        {
            get
            {
                if (Concatenated != null)
                {
                    return Elements.Count + Concatenated.TotalCount;
                }
                else return Elements.Count;
            }
        }
        public override double SpecificCount => Elements.Count;
        
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
            if (GetElementType() == sequence.ElementType || ElementType == GSharpType.EMPTY)
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
            else
            {
                throw new GSharpError(ErrorType.SEMANTIC, $"Cannot concatenate a sequence of type {sequence.ElementType} to a sequence of type {GetElementType()}.");
            }
        }
        public override string ToString()
        {
            string result = "";
            if (Elements.Count == 0)
            {
                result = "{ }";
            }
            else
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

        public override Sequence GetSequenceTail(double index)
        {
            if (Concatenated != null)
                return new FiniteSequence(Elements.GetRange((int)index, Elements.Count - 1), Concatenated);
            else return new FiniteSequence(Elements.GetRange((int)index, Elements.Count - 1));
        }
    }
    /// <summary>
    /// Represents an infinite sequence of expressions of integers.
    /// </summary>
    public class InfiniteSequence : Sequence, IEnumerable<Expression>, IGSharpObject
    {
        public GSharpType Type => GSharpType.SEQUENCE;
        public GSharpType ElementType => GSharpType.NUMBER;
        public GSharpNumber Start { get; private set; }

        public InfiniteSequence(GSharpNumber start)
        {
            Start = start;
        }
        public InfiniteSequence(GSharpNumber start, Sequence concatenated)
        {
            Start = start;
            Concatenated = concatenated;
        }
        public override double TotalCount => double.PositiveInfinity;
        public override double SpecificCount => double.PositiveInfinity;
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
            string result = $"{{ {Start}... }}";
            if (Concatenated != null)
            {
                result += " + " + Concatenated.ToString();
            }
            return result;
        }

        public override void Concatenate(Sequence sequence)
        {
            if (GetElementType() == sequence.ElementType || ElementType == GSharpType.EMPTY)
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
            else throw new GSharpError(ErrorType.SEMANTIC, $"Cannot concatenate a sequence of type {sequence.ElementType} to a sequence of type {GetElementType()}.");
        }

        public override Sequence GetSequenceTail(double index)
        {
            if (Concatenated != null)
                return new InfiniteSequence(new GSharpNumber(Start.Value + index), Concatenated);
            else return new InfiniteSequence(new GSharpNumber(Start.Value + index));
        }
    }
    /// <summary>
    /// Represents a range sequence of expressions of integers.
    /// </summary>
    public class RangeSequence : Sequence, IEnumerable<Expression>, IGSharpObject
    {
        public GSharpType Type => GSharpType.SEQUENCE;
        public GSharpType ElementType => GSharpType.NUMBER;
        public GSharpNumber Start { get; }
        public GSharpNumber End { get; }
        public override double TotalCount
        {
            get
            {
                if (Concatenated != null)
                {
                    if (End.Value - Start.Value >= 0) return (End.Value - Start.Value) + 1 + Concatenated.TotalCount;
                    else return Concatenated.TotalCount;
                }
                if (End.Value - Start.Value >= 0) return (End.Value - Start.Value) + 1;
                else return 0;
            }
        }
        public override double SpecificCount
        {
            get
            {
                if (End.Value - Start.Value >= 0) return (End.Value - Start.Value) + 1;
                else return 0;
            }
        }
        public RangeSequence(GSharpNumber start, GSharpNumber end)
        {
            Start = start;
            End = end;
        }
        public RangeSequence(GSharpNumber start, GSharpNumber end, Sequence concatenated)
        {
            Start = start;
            End = end;
            Concatenated = concatenated;
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
            if (GetElementType() == sequence.ElementType || ElementType == GSharpType.EMPTY)
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
            else
            {
                throw new GSharpError(ErrorType.SEMANTIC, $"Cannot concatenate a sequence of type {sequence.ElementType} to a sequence of type {GetElementType()}.");
            }
        }

        public override Sequence GetSequenceTail(double index)
        {
            if (Concatenated != null)
                return new RangeSequence(new GSharpNumber(Start.Value + index), End, Concatenated);
            else
            return new RangeSequence(new GSharpNumber(Start.Value + index), End);
        }
    }
    public class FiniteSequenceExpression : Expression
    {
        public GSharpType Type => GSharpType.SEQUENCE;
        public List<Expression> Elements { get; }
        public FiniteSequenceExpression(List<Expression> elements)
        {
            Elements = elements;
        }
    }
}
