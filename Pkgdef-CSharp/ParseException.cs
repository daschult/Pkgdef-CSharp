using System;

namespace Pkgdef_CSharp
{
    internal class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        {
        }
    }
}
