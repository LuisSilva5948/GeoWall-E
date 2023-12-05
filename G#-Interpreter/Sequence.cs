using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    public class Sequence
    {
        public List<Expression> Expressions { get; }

        public Sequence(List<Expression> expressions)
        {
            Expressions = expressions;
        }
    }
}
