using EntitySqlBuilder.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder
{
    public class Entity
    {
        private bool _locked;

        internal List<EntityParameter<object>> Parameters { get; private set; } = new();
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; init; }

        internal bool Locked
        {
            get => _locked;
            set
            {
                if (_locked)
                    throw new Exception("Entity can't be unlocked after locking.");
                _locked = value;
            }
        }
        private Entity(string name)
        {
            Name = name;
        }
        public static Entity GetEntity(string name)
        {
            var entity = EntityStorage.GetEmptyEntity(name) ?? new Entity(name);
            return entity;
        }

        internal Entity Clone()
        {
            var clone = (Entity)MemberwiseClone();
            clone.Id = Guid.NewGuid().ToString();

            var newParameters = Parameters.Select(entityParameter => entityParameter.Clone()).ToList();

            clone.Parameters = newParameters;
            return clone;
        }

        internal void ConfigureParam(string name, Type type, bool isKey = false)
        {
            if (Locked)
                throw new Exception("Entity can't be configured after build completes.");
            if (Parameters.Exists(param => string.Compare(name, param.Info.Name, StringComparison.Ordinal) == 0))
                throw new Exception("Entity can't be configured after build completes.");

            var paramInfo = new EntityParameterInfo()
            {
                Name = name,
                ParamType = type,
                IsKey = isKey
            };
            var param = new EntityParameter<object>(paramInfo);
            Parameters.Add(param);
        }

        public bool HasParam(string name)
        {
            return Parameters.Any(param => string.Compare(param.Info.Name, name, StringComparison.Ordinal) == 0);
        }
        public T GetParam<T>(string name)
        {
            return (T)Parameters.Find(param => string.Compare(param.Info.Name, name, StringComparison.Ordinal) == 0)?
                .CurrentValue.Value;
        }
        internal void SetParam(string name, EntityParameter<object> value)
        {
            CheckParameterConfiguration(name);
            Parameters.Add(value);
        }
        public void SetParam(string name, object newValue)
        {
            CheckParameterConfiguration(name);
            var parameter = Parameters.Find(param => string.Compare(param.Info.Name, name, StringComparison.Ordinal) == 0);
            if (parameter != null)
                parameter.CurrentValue = new Emptiable<object>(newValue);
        }


        private void CheckParameterConfiguration(string name)
        {
            if (!Parameters.Exists(param => string.Compare(param.Info.Name, name, StringComparison.Ordinal) == 0))
                throw new Exception($"Parameter named {name} not configured.");
        }
    }
}
