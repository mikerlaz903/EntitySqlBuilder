using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Exceptions
{
    public class NoDataException : Exception
    {
        public NoDataException()
        {

        }
        public NoDataException(string message) : base(message)
        {

        }
        public NoDataException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
