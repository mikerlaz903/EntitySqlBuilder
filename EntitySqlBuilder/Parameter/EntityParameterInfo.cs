using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Parameter
{
    internal class EntityParameterInfo
    {
        public string Name { get; set; }
        public bool IsKey { get; set; }

        public Type ParamType { get; init; }

    }
}
