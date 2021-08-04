using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Exceptions
{
    public class ConfigureLockedParameterException : Exception
    {
        public ConfigureLockedParameterException()
        {

        }
        public ConfigureLockedParameterException(string message) : base(message)
        {

        }
        public ConfigureLockedParameterException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
