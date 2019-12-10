using System;
using System.Collections.Generic;
using System.Text;

namespace Poise.Exceptions
{
    public class MultipleShimsFoundException : Exception
    {
        public override string Message =>
            "If you have overloaded methods, you need to provide their parameters ending in their return type. Use the other GetShim method";
    }
}
