using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Exceptions
{
    public class UndefinedParameterNameException : Exception
    {
        public UndefinedParameterNameException()
        {

        }
        public UndefinedParameterNameException(string message) : base(message)
        {

        }
        public UndefinedParameterNameException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
