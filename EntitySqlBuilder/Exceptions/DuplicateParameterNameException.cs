using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Exceptions
{
    public class DuplicateParameterNameException : Exception
    {
        public DuplicateParameterNameException()
        {

        }
        public DuplicateParameterNameException(string message) : base(message)
        {

        }
        public DuplicateParameterNameException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
