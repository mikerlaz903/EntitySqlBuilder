using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder
{
    internal static class EntityComparator
    {
        public static bool Compare<T>(T item1, T item2)
        {
            if (typeof(T).GetInterfaces().Contains(typeof(IEquatable<object>)))
            {
                return ((IEquatable<object>)item1).Equals((IEquatable<object>)item2);
            }
            if (typeof(T).IsValueType)
            {
                return item1.Equals(item2);
            }

            var stringLeft = item1?.ToString();
            var stringRight = item2?.ToString();
            return string.Compare(stringLeft, stringRight, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
