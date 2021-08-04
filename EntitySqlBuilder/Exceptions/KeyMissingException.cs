using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Exceptions
{
    public class KeyMissingException : Exception
    {
        public KeyMissingException()
        {

        }
        public KeyMissingException(string message) : base(message)
        {

        }
        public KeyMissingException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
