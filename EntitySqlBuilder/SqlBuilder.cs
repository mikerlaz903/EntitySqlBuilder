using EntitySqlBuilder.Exceptions;
using EntitySqlBuilder.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySqlBuilder
{
    public class SqlUpdateBuilder
    {
        private Entity _entity;

        public char StringSymbol = '\'';
        public string SqlPattern = "update {table} set {changed_fields} where {key_field} = {key_value}";

        public readonly string TablePattern = "{table}";
        public readonly string ChangedFieldsPattern = "{changed_fields}";
        public readonly string KeyFieldPattern = "{key_field}";
        public readonly string KeyValuePattern = "{key_value}";

        public string UpdateSql { get; private set; }

        private SqlUpdateBuilder()
        {
        }

        public SqlUpdateBuilder(Entity entity) :
            this()
        {
            _entity = entity;
            CreateSql();
        }

        private void CreateSql()
        {
            var table = _entity.Name;
            var changedFields = GetParamString();

            var (keyField, entityParameter) = GetKey();
            var keyValue = entityParameter.CurrentValue?.ToString();

            if (string.IsNullOrWhiteSpace(changedFields))
            {
                if ((_entity.Options & EntityUpdaterOptions.ThrowExceptionIfUpdatableFieldMissing) ==
                    EntityUpdaterOptions.ThrowExceptionIfUpdatableFieldMissing)
                    throw new UpdatableFieldMissingException("No fields was modified.");
                return;
            }

            if (string.IsNullOrWhiteSpace(keyValue))
            {
                if ((_entity.Options & EntityUpdaterOptions.ThrowExceptionIfKeyMissing) ==
                    EntityUpdaterOptions.ThrowExceptionIfKeyMissing)
                    throw new KeyMissingException("Undefined key while composing an update.");
                return;
            }

            UpdateSql = SqlPattern.
                Replace(TablePattern, table).
                Replace(ChangedFieldsPattern, changedFields).
                Replace(KeyFieldPattern, keyField).
                Replace(KeyValuePattern, keyValue);
        }
        private IEnumerable<EntityParameter<object>> GetModifiedParams()
        {
            return new List<EntityParameter<object>>(_entity.Parameters.Where(param => param.IsModified));
        }
        private string GetParamString()
        {
            List<string> paramString = new();

            foreach (var param in GetModifiedParams())
            {
                var name = param.Info.Name;
                var value = param.CurrentValue;

                if (param.Info.ParamType == typeof(string) || param.Info.ParamType == typeof(DateTime))
                    paramString.Add($"{name} = {StringSymbol}{value}{StringSymbol}");
                else
                    paramString.Add($"{name} = {value}");
            }

            return string.Join(", ", paramString);
        }


        private (string, EntityParameter<object>) GetKey()
        {
            var parameter = _entity.Parameters.First(param => param.Info.IsKey);
            return (parameter.Info.Name, parameter);
        }

    }
}
