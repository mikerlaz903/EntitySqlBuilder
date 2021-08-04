using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Exceptions
{
    public class ModifyingValueIncorrectTypeException : Exception
    {
        public ModifyingValueIncorrectTypeException()
        {

        }
        public ModifyingValueIncorrectTypeException(string message) : base(message)
        {

        }
        public ModifyingValueIncorrectTypeException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
