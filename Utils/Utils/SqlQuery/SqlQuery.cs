using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Parser.Utils.SqlQuery
{
    public class SqlQuery
    {
        public class PropertyBind
        {
            public PropertyInfo Property { get; set; }
            public string Name { get; set; }

            public Accessor Accessor { get; set; }
        }

        public static object Convert(object sourceValue, Type destinationType)
        {
            // Nullable Type
            destinationType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

            if (sourceValue.GetType().Equals(destinationType))
            {
                if (sourceValue is DateTime && destinationType == typeof(DateTime))
                {
                    var time = System.Convert.ToDateTime(sourceValue);
                    var returnValue = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Utc);
                    return returnValue;
                }
                return sourceValue;
            }
            else if (sourceValue is string && destinationType == typeof(bool))
            {
                var s = sourceValue as string;
                if (string.IsNullOrEmpty(s))
                    return false;
                switch (s.Trim().ToUpper())
                {
                    case "TRUE":
                    case "YES":
                    case "1":
                    case "-1":
                    case "ON":
                    case "ADD":
                        return true;

                    default:
                        return false;
                }
            }
            else if (sourceValue is string && destinationType == typeof(Int32))
            {

                int returnValue = 0;
                try
                {
                    returnValue = System.Convert.ToInt32(System.Convert.ToDouble(sourceValue, System.Globalization.CultureInfo.InvariantCulture));
                }
                catch { }
                return returnValue;
            }
            else if (sourceValue is string && destinationType == typeof(Decimal))
            {

                decimal returnValue = 0;
                try
                {
                    returnValue = System.Convert.ToDecimal(sourceValue, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {

                }
                return returnValue;
            }
            else if (sourceValue is string && destinationType == typeof(Guid))
            {
                var s = sourceValue as string;
                Guid outValue;
                Guid.TryParse(s, out outValue);
                return outValue;
            }
            else if (sourceValue is string && destinationType == typeof(Double))
            {

                Double returnValue = 0;
                try
                {
                    returnValue = System.Convert.ToDouble(sourceValue, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {

                }
                return returnValue;
            }
            else if (destinationType.BaseType != null && destinationType.BaseType == typeof(Enum))
            {
                if (sourceValue is string)
                {
                    int tempValue;
                    if (int.TryParse((string)sourceValue, out tempValue))
                    {
                        foreach (var item in Enum.GetValues(destinationType))
                        {
                            if ((int)item == tempValue)
                                return item;
                        }
                        return 0;
                    }
                    else
                        return Enum.Parse(destinationType, (string)sourceValue);
                }
                else if (sourceValue is int)
                {
                    foreach (var item in Enum.GetValues(destinationType))
                    {
                        if ((int)item == (int)sourceValue)
                            return item;
                    }
                    return 0;
                }
                else if (sourceValue is byte)
                {
                    foreach (var item in Enum.GetValues(destinationType))
                    {
                        if ((byte)item == (byte)sourceValue)
                            return item;
                    }
                    return 0;
                }
            }
            try
            {
                return System.Convert.ChangeType(sourceValue, destinationType);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task SendQueryAsync(string connectionString, string procedureName, params KeyValuePair<string, object>[] parameters)
        {
            string traceParamaters = String.Empty;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection))
                {
                    if (parameters != null)
                        foreach (var p in parameters)
                        {
                            traceParamaters += "@" + p.Key + " = " + p.Value + ",";
                            sqlCommand.Parameters.AddWithValue("@" + p.Key, p.Value);
                        }
                    Trace.WriteLine(procedureName, traceParamaters);

                    sqlCommand.CommandTimeout = 14000; // устанавливаем 2 часа на выполнение этой операции
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    await sqlCommand.ExecuteScalarAsync();
                }
            }
        }

        public static async Task<IEnumerable<T>> SendQueryAsync<T>(string connectionString, string procedureName, params KeyValuePair<string, object>[] parameters)
        {
            string traceParamaters = String.Empty;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection))
                {
                    if (parameters != null)
                        foreach (var p in parameters)
                        {
                            traceParamaters += "@" + p.Key + " = " + p.Value + ",";
                            sqlCommand.Parameters.AddWithValue("@" + p.Key, p.Value);
                        }

                    //sqlCommand.CommandTimeout = 14400; // устанавливаем 20 минут на выполнение этой операции
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    return await ReaderAsync<T>(sqlCommand);

                }
            }
        }

        private static async Task<List<T>> ReaderAsync<T>(SqlCommand sqlCommand)
        {
            List<T> result = new List<T>();
            using (var reader = await sqlCommand.ExecuteReaderAsync().ConfigureAwait(false))
            {
                var properties = GetAvaiableProperties(reader, typeof(T));

                ConstructorInfo ctor = typeof(T).GetConstructors().First();
                ObjectActivator<T> createdActivator = Activator<T>(ctor);

                if (properties.Count > 0)
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        T value = ReaderValue(reader, properties, createdActivator);

                        result.Add(value);
                    }
            }

            return result;
        }

        private static T ReaderValue<T>(SqlDataReader reader, List<PropertyBind> properties, ObjectActivator<T> createdActivator)
        {
            T value = createdActivator();
            properties.ForEach(prop =>
            {
                try
                {
                    if (reader[prop.Name] != DBNull.Value)
                        prop.Accessor.SetValue(value, Convert(reader[prop.Name], prop.Property.PropertyType));
                }
                catch (Exception ex)
                {

                }
            });
            return value;
        }

        delegate T ObjectActivator<T>(params object[] args);

        private static ObjectActivator<T> Activator<T>(ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");
            Expression[] argsExp = new Expression[paramsInfo.Length];

            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;
                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                argsExp[i] = paramCastExp;
            }

            NewExpression newExp = Expression.New(ctor, argsExp);
            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);
            ObjectActivator<T> compiled = (ObjectActivator<T>)lambda.Compile();
            return compiled;
        }

        private static List<PropertyBind> GetAvaiableProperties(SqlDataReader reader, Type classType)
        {
            var allProps = classType.GetProperties().ToList();
            var properties = new List<PropertyBind>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var property = new PropertyBind();
                var readerName = reader.GetName(i);
                var propertyName = readerName;

                PropertyInfo c = null;

                if (c == null)
                {
                    c = allProps.Find(p => p.GetCustomAttributes(typeof(RecordName), true).Count() > 0 && string.Compare(((RecordName)p.GetCustomAttributes(typeof(RecordName), true)[0]).Name, readerName, StringComparison.OrdinalIgnoreCase) == 0);
                    if (c != null)
                        propertyName = ((RecordName)c.GetCustomAttributes(typeof(RecordName), true).FirstOrDefault()).Name;
                }

                if (c == null)
                    c = allProps.Find(p => string.Compare(p.Name, readerName, StringComparison.OrdinalIgnoreCase) == 0);

                if (c != null)
                {
                    var ignoreAttr = (c.GetCustomAttributes(typeof(IgnoreBind), true) as IgnoreBind[]).ToList();
                    if (!ignoreAttr.Exists(a => a.Action == Action.RecordSet))
                    {
                        property.Name = propertyName;
                        property.Property = c;
                        property.Accessor = new Accessor(c);
                        properties.Add(property);
                    }
                }
            }
            return properties;
        }
    }
}
