using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Exceptions
{
    public class UpdatableFieldMissingException : Exception
    {
        public UpdatableFieldMissingException()
        {

        }
        public UpdatableFieldMissingException(string message) : base(message)
        {

        }
        public UpdatableFieldMissingException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
