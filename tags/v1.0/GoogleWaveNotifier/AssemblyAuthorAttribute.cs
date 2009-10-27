using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleWaveNotifier
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited=false)]
    public class AssemblyAuthorAttribute: Attribute
    {
        public string Author { get; private set; }

        public AssemblyAuthorAttribute(string author)
        {
            Author = author;
        }
    }
}
