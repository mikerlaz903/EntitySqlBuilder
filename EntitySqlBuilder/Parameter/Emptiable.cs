using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder.Parameter
{
    internal class Emptiable<T>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        public bool HasValue => _hasValue;
        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new Exception("No value");
                }
                return _value;
            }
        }

        public Emptiable(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public Emptiable()
        {
            _hasValue = false;
        }


        public override string? ToString()
        {
            if (_hasValue)
            {
                if (_value != null)
                    return _value.ToString();
                else
                    return "null";
            }

            return "";
        }

        public override bool Equals(object? obj)
        {
            if (!_hasValue || (_hasValue && Value == null))
                return false;
            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
