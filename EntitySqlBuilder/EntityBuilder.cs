using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder
{
    public class EntityBuilder
    {
        private readonly Entity _entity;

        public EntityBuilder(string name)
        {
            _entity = Entity.GetEntity(name);
        }

        public EntityBuilder ConfigureParameter(string name, Type type, bool isKey = false)
        {
            _entity.ConfigureParam(name, type, isKey);

            return this;
        }
        public void Build()
        {
            _entity.Locked = true;
            EntityStorage.EmptyEntityCollection.Add(_entity);
        }
    }
}
