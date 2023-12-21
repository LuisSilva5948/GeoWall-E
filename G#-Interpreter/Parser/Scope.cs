using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharpInterpreter
{
    public class Scope
    {
        public Dictionary<string, object> Variables { get; private set; }
        public Scope Parent { get; private set; }
        public Scope()
        {
            Variables = new Dictionary<string, object>();
            Parent = null;
        }
        public void AddVariable(string identifier, object value)
        {
            Variables[identifier] = value;
        }
        public Scope GetVariable(string identifier)
        {
            if (Variables.ContainsKey(identifier))
                return this;
            else if (Parent != null)
                return Parent.GetVariable(identifier);
            else
                return null;
        }
        public Scope BuildChildScope()
        {
            Scope child = new Scope();
            child.Parent = this;
            return child;
        }
    }
}
