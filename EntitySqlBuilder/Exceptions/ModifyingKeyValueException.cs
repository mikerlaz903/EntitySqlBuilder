using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Exceptions
{
    public class ModifyingKeyValueException : Exception
    {
        public ModifyingKeyValueException()
        {

        }
        public ModifyingKeyValueException(string message) : base(message)
        {

        }
        public ModifyingKeyValueException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
