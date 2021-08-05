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
        private readonly Entity _entity;

        public char StringSymbol = '\'';

        public string UpdateSqlPattern = "update {table} set {changed_fields} where {key_field} = {key_value}";
        public string InsertSqlPattern = "insert into {table}({changed_fields_names}) values ({changed_fields_values})";

        public readonly string TablePattern = "{table}";
        public readonly string ChangedFieldsPattern = "{changed_fields}";
        public readonly string ChangedFieldNamesPattern = "{changed_fields_names}";
        public readonly string ChangedFieldValuesPattern = "{changed_fields_values}";
        public readonly string KeyFieldPattern = "{key_field}";
        public readonly string KeyValuePattern = "{key_value}";

        public string UpdateSql { get; private set; }
        public string InsertSql { get; private set; }

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

            var paramsWithValues = GetParams();
            var changedFieldNames = string.Join(", ", paramsWithValues.Select(param => param.Item1));
            var changedFieldValues = string.Join(", ", paramsWithValues.Select(param => param.Item2));

            if (string.IsNullOrWhiteSpace(changedFieldValues))
            {
                if ((_entity.Options & EntityUpdaterOptions.ThrowExceptionIfFieldMissing) ==
                    EntityUpdaterOptions.ThrowExceptionIfFieldMissing)
                    throw new NoDataException("No fields to insert.");
                return;
            }

            InsertSql = InsertSqlPattern.
                Replace(TablePattern, table).
                Replace(ChangedFieldNamesPattern, changedFieldNames).
                Replace(ChangedFieldValuesPattern, changedFieldValues);

            var (keyField, entityParameter) = GetKey();
            var keyValue = entityParameter.CurrentValue?.ToString();

            var updatedParams = GetUpdatedParams();


            var changedFields = string.Join(", ", updatedParams.Select(param => $"{param.Item1} = {param.Item2}"));


            if (string.IsNullOrWhiteSpace(changedFields))
            {
                if ((_entity.Options & EntityUpdaterOptions.ThrowExceptionIfFieldMissing) ==
                    EntityUpdaterOptions.ThrowExceptionIfFieldMissing)
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

            UpdateSql = UpdateSqlPattern.
                Replace(TablePattern, table).
                Replace(ChangedFieldsPattern, changedFields).
                Replace(KeyFieldPattern, keyField).
                Replace(KeyValuePattern, keyValue);
        }
        private IEnumerable<EntityParameter<object>> GetParamsWithValues()
        {
            return new List<EntityParameter<object>>(_entity.Parameters.Where(param => param.CurrentValue.HasValue));
        }
        private IEnumerable<EntityParameter<object>> GetModifiedParams()
        {
            return new List<EntityParameter<object>>(_entity.Parameters.Where(param => param.IsModified));
        }

        private List<(string, string)> GetParams()
        {
            List<(string, string)> paramString = new();

            foreach (var param in GetParamsWithValues().OrderBy(param => !param.Info.IsKey).ThenBy(param => param.Info.Name))
            {
                var name = param.Info.Name;
                var value = param.CurrentValue;

                if (param.Info.ParamType == typeof(string) || param.Info.ParamType == typeof(DateTime))
                    paramString.Add(($"{name}", $"{StringSymbol}{value}{StringSymbol}"));
                else
                    paramString.Add(($"{name}", $"{value}"));
            }

            return paramString;
        }
        private List<(string, string)> GetUpdatedParams()
        {
            List<(string, string)> paramString = new();

            foreach (var param in GetModifiedParams().OrderBy(param => param.Info.Name))
            {
                var name = param.Info.Name;
                var value = param.CurrentValue;

                if (param.Info.ParamType == typeof(string) || param.Info.ParamType == typeof(DateTime))
                    paramString.Add(($"{name}", $"{StringSymbol}{value}{StringSymbol}"));
                else
                    paramString.Add(($"{name}", $"{value}"));
            }

            return paramString;
        }


        private (string, EntityParameter<object>) GetKey()
        {
            var parameter = _entity.Parameters.First(param => param.Info.IsKey);
            return (parameter.Info.Name, parameter);
        }

    }
}
