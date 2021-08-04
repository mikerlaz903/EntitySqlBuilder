using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder
{
    [Flags]
    public enum EntityUpdaterOptions : ushort
    {
        ThrowExceptionIfKeyMissing = 1,
        ThrowExceptionIfUpdatableFieldMissing = 2,
        IgnoreEntityAndParameterNameCase = 4
    }
}
