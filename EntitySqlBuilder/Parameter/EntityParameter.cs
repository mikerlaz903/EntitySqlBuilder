using EntitySqlBuilder.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Parameter
{
    internal class EntityParameter<T>
    {
        private Emptiable<T> _currentValue = new();

        public EntityParameterInfo Info { get; init; }

        public bool IsModified { get; set; }
        public Emptiable<T> InitialValue { get; internal set; } = new();
        public Emptiable<T> CurrentValue
        {
            get => _currentValue.HasValue ? _currentValue : InitialValue;
            set
            {
                if (value.Value != null && Info.ParamType != value.Value?.GetType())
                    throw new ModifyingValueIncorrectTypeException($"Modified value must be same type. Target type is {Info.ParamType}, value type is {value.Value?.GetType()}");
                if (!InitialValue.HasValue)
                {
                    InitialValue = value;
                    return;
                }

                if (EntityComparator.Compare(CurrentValue, value))
                    return;
                if (Info.IsKey)
                    throw new ModifyingKeyValueException("Key field can't be modified.");

                _currentValue = value;

                IsModified = !EntityComparator.Compare(InitialValue, value);
            }
        }

        internal EntityParameter(EntityParameterInfo info)
        {
            Info = info;
        }

        internal EntityParameter(string name, Emptiable<T> value)
        {
            Info = new EntityParameterInfo()
            {
                Name = name,
                ParamType = value.GetType()
            };

            InitialValue = value;
        }
        internal EntityParameter(string name, Emptiable<T> value, bool isKey)
            : this(name, value)
        {
            Info.IsKey = isKey;
        }

        internal EntityParameter<T> Clone()
        {
            return (EntityParameter<T>)MemberwiseClone();
        }
    }
}
