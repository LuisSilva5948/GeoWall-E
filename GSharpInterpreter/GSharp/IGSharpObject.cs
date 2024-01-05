using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharpInterpreter
{
    public interface IGSharpObject
    {
        GSharpType Type { get; }
    }
}
