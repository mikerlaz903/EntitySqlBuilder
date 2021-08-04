using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder
{
    public static class EntityStorage
    {
        internal static List<Entity> EmptyEntityCollection { get; set; } = new();

        internal static Entity GetEmptyEntity(string entityName, bool ignoreCase)
        {
            var comparison = ignoreCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            return EmptyEntityCollection.Find(emptyEntity =>
                string.Compare(emptyEntity.Name, entityName, StringComparison.Ordinal) == 0)?.Clone();
        }
    }
}
