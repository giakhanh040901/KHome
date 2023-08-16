using Dapper;
using Dapper.Oracle;
using EPIC.DataAccess.Models;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using SHB.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EPIC.DataAccess
{
    public abstract class SQLHelper
    {
        protected string ConnectionString;
        protected IDbConnection Connection;
        protected ILogger Logger;

        public const int ORACLE_WRONG_PARAMETERS = 6550;
        public const int ORACLE_USER_HANDLED_EXCEPTION_CODE = 20000;

        public const int ERR_SQL_BASE = 200;
        public const int ERR_SQL_OPEN_CONNECTION_FAIL = ERR_SQL_BASE + 1;
        public const int ERR_SQL_EXECUTE_COMMAND_FAIL = ERR_SQL_BASE + 2;
        public const int ERR_SQL_DISCOVERY_PARAMS_FAIL = ERR_SQL_BASE + 3;
        public const int ERR_SQL_ASSIGN_PARAMS_FAIL = ERR_SQL_BASE + 4;

        public void OpenConnection()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public void CloseConnection()
        {
            Connection.Close();
        }

        public ConnectionState Status()
        {
            return Connection.State;
        }

        public IEnumerable<T> ExecuteCommandText<T>(string command, object parameters)
        {
            try
            {
                Logger.LogInformation("ExecuteCommandText: {0}, param: {1}", command, parameters != null ? JsonSerializer.Serialize(parameters) : "");
                OpenConnection();
                var dynamicParams = ObjectToParameters(parameters);
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                return Connection.Query<T>(command, dynamicParams, commandType: CommandType.Text);
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, command);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ExecuteCommandText: {}", command);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public T ExecuteCommandTextToFirst<T>(string command, object parameters)
        {
            try
            {
                Logger.LogInformation("ExecuteCommandText: {0}, {1}", command, parameters != null ? JsonSerializer.Serialize(parameters) : "");
                OpenConnection();
                var dynamicParams = ObjectToParameters(parameters);
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                return Connection.QueryFirstOrDefault<T>(command, dynamicParams, commandType: CommandType.Text);
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, command);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ExecuteCommandText: {}", command);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public int ExecuteCommandTextNonQuery(string command, object parameters)
        {
            return Connection.Execute(command, parameters, commandType: CommandType.Text);
        }

        public IEnumerable<T> ExecuteProcedure<T>(string procedureName, object parameters)
        {
            try
            {
                Logger.LogInformation("ExecuteProcedure: {0}, param: {1}", procedureName, parameters != null ? JsonSerializer.Serialize(parameters) : "");
                OpenConnection();
                var dynamicParams = ObjectToParameters(parameters);
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                return Connection.Query<T>(procedureName, dynamicParams, commandType: CommandType.StoredProcedure);
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public IDbTransaction BeginTransaction()
        {
            OpenConnection();
            return Connection.BeginTransaction();
        }

        public void CommitTransaction(IDbTransaction transaction)
        {
            transaction.Commit();
        }

        public void ExecuteProcedureArrayObject(string procedureName, object parameters, List<string> listProf)
        {

            //var command = new OracleCommand("BEGIN Testpackage.Testprocedure(:Areas); END;", Connection);

            //Connection.Open();

            //var arry = command.Parameters.Add("pv_PROF_FILE_URL", OracleDbType.Varchar2);

            //arry.Direction = ParameterDirection.Input;
            //arry.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            //arry.Value = listProf.ToArray();
            //arry.Size = listProf.Count();
            //arry.ArrayBindSize = listProf.Select(_ => _.Length).ToArray();
            //arry.ArrayBindStatus = Enumerable.Repeat(OracleParameterStatus.Success, listProf.Count()).ToArray();

            //command.Parameters.Add("cur", /*dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output*/);

            //command.ExecuteNonQuery();

            //Connection.Close();
            try
            {
                Logger.LogInformation("ExecuteProcedure: {0}, param: {1}", procedureName, parameters != null ? JsonSerializer.Serialize(parameters) : "");
                OpenConnection();
                var dynamicParams = ArrayObjectToParameters(parameters);
                dynamicParams.Add("pv_PROF_FILE_URL", collectionType: OracleMappingCollectionType.PLSQLAssociativeArray, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public int ExecuteProcedureDynamicParams(string procedureName, OracleDynamicParameters dynamicParams, bool closeConnection = true)
        {
            try
            {
                Logger.LogInformation("ExecuteProcedure: {0}, param: {1}", procedureName, dynamicParams != null ? JsonSerializer.Serialize(dynamicParams) : "");
                OpenConnection();
                List<object> listParam = new();
                return Connection.Execute(procedureName, dynamicParams, commandType: CommandType.StoredProcedure);
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                if (closeConnection)
                {
                    CloseConnection();
                }
            }
        }

        public T ExecuteProcedureDynamicParamsToFirst<T>(string procedureName, OracleDynamicParameters dynamicParams, bool closeConnection = true)
        {
            try
            {
                Logger.LogInformation("ExecuteProcedure: {0}, param: {1}", procedureName, dynamicParams != null ? JsonSerializer.Serialize(dynamicParams) : "");
                OpenConnection();
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                return Connection.QueryFirstOrDefault<T>(procedureName, dynamicParams, commandType: CommandType.StoredProcedure);

            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                if (closeConnection)
                {
                    CloseConnection();
                }
            }
        }

        public PagingResult<T> ExecuteProcedurePaging<T>(string procedureName, object parameters)
        {
            try
            {
                Logger.LogInformation("ExecuteProcedure: {0}, param: {1}", procedureName, parameters != null ? JsonSerializer.Serialize(parameters) : "");
                OpenConnection();
                var dynamicParams = ObjectToParameters(parameters);
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                dynamicParams.Add("totalItems", dbType: OracleMappingType.Decimal, direction: ParameterDirection.Output);

                var result = Connection.Query<T>(procedureName, dynamicParams, commandType: CommandType.StoredProcedure);

                var totalItems = dynamicParams.Get<decimal>("totalItems");

                return new PagingResult<T>
                {
                    Items = result,
                    TotalItems = totalItems,
                };
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public T ExecuteProcedureToFirst<T>(string procedureName, object parameters, bool closeConnection = true)
        {
            try
            {
                Logger.LogInformation("ExecuteProcedure: {0}, param: {1}", procedureName, parameters != null ? JsonSerializer.Serialize(parameters) : "");
                OpenConnection();
                var dynamicParams = ObjectToParameters(parameters);
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                return Connection.QueryFirstOrDefault<T>(procedureName, dynamicParams, commandType: CommandType.StoredProcedure);
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                if (closeConnection)
                {
                    CloseConnection();
                }
            }
        }

        public int ExecuteProcedureNonQuery(string procedureName, object parameters, bool closeConnection = true)
        {
            try
            {
                Logger.LogInformation("ExecuteProcedure: {0}, param: {1}", procedureName, parameters != null ? JsonSerializer.Serialize(parameters) : "");
                OpenConnection();
                return Connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                if (closeConnection)
                {
                    CloseConnection();
                }
            }
        }

        public List<Dictionary<string, object>> ExecuteProcedureToDynamicData(string procedureName, Dictionary<string, dynamic> parameters)
        {
            try
            {
                Logger.LogInformation("ExecuteProcedure: {0}, param: {1}", procedureName, parameters != null ? JsonSerializer.Serialize(parameters) : "");
                OpenConnection();
                var dynamicParams = ObjectToParameters(parameters);
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                var rows = new List<Dictionary<string, object>>();
                using (var reader = Connection.ExecuteReader(procedureName, dynamicParams, commandType: CommandType.StoredProcedure))
                {
                    rows = DataReaderToDynamicData(reader);
                }
                return rows;
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public PagingResult<T> ExecuteProcedurePaging<T>(string procedureName, int pageSize, int currentPage, Dictionary<string, dynamic> parameters)
        {
            try
            {
                OpenConnection();
                Logger.LogInformation("ExecuteProcedure: {}", procedureName);
                var dynamicParams = ObjectToParameters(parameters);
                dynamicParams.Add("pageSize", dbType: OracleMappingType.Int32, direction: ParameterDirection.Input, value: pageSize);
                dynamicParams.Add("currentPage", dbType: OracleMappingType.Int32, direction: ParameterDirection.Input, value: currentPage);
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                dynamicParams.Add("totalItems", dbType: OracleMappingType.Decimal, direction: ParameterDirection.Output);

                var rows = Connection.Query<T>(procedureName, dynamicParams, commandType: CommandType.StoredProcedure);

                var totalItems = dynamicParams.Get<decimal>("totalItems");

                return new PagingResult<T>
                {
                    Items = rows.ToList(),
                    TotalItems = totalItems
                };
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public PagingResult<Dictionary<string, object>> ExecuteProcedureToDynamicDataPaging(string procedureName, int pageSize, int currentPage, Dictionary<string, dynamic> parameters)
        {
            try
            {
                OpenConnection();
                Logger.LogInformation("ExecuteProcedure: {}", procedureName);
                var dynamicParams = ObjectToParameters(parameters);
                dynamicParams.Add("pageSize", dbType: OracleMappingType.Int32, direction: ParameterDirection.Input, value: pageSize);
                dynamicParams.Add("currentPage", dbType: OracleMappingType.Int32, direction: ParameterDirection.Input, value: currentPage);
                dynamicParams.Add("cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                dynamicParams.Add("totalItems", dbType: OracleMappingType.Decimal, direction: ParameterDirection.Output);

                //var result = Connection.ExecuteReader(procedureName, dynamicParams, commandType: CommandType.StoredProcedure);

                var rows = new List<Dictionary<string, object>>();
                using (var reader = Connection.ExecuteReader(procedureName, dynamicParams, commandType: CommandType.StoredProcedure))
                {
                    rows = DataReaderToDynamicData(reader);
                }

                var totalItems = dynamicParams.Get<decimal>("totalItems");

                return new PagingResult<Dictionary<string, object>>
                {
                    Items = rows.ToList(),
                    TotalItems = totalItems
                };
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataSet ExecuteProcedureToDataSet(string procedureName, List<string> cursorList, Dictionary<string, dynamic> parameters)
        {
            try
            {
                OpenConnection();
                Logger.LogInformation("ExecuteProcedure: {0}, param: {1}", procedureName, parameters != null ? JsonSerializer.Serialize(parameters) : "");
                var dynamicParams = ObjectToParameters(parameters);
                foreach (var item in cursorList)
                {
                    dynamicParams.Add(item, dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                }

                var reader = Connection.ExecuteReader(procedureName, dynamicParams, commandType: CommandType.StoredProcedure);
                var dataset = ConvertDataReaderToDataSet(reader);
                return dataset;

            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, procedureName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CommandText: {}", procedureName);
                throw CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        protected OracleDynamicParameters ObjectToParameters(object param)
        {
            var parameters = new OracleDynamicParameters();
            if (param == null)
                return parameters;
            
            var properties = param.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var key = prop.Name;
                var value = prop.GetValue(param);

                parameters.Add(key, value);
            }
            return parameters;
        }

        protected OracleDynamicParameters ArrayObjectToParameters(object param)
        {
            var parameters = new OracleDynamicParameters();
            if (param == null)
                return parameters;

            var properties = param.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var key = prop.Name;
                var value = prop.GetValue(param);

                parameters.Add(key, value);
            }
            return parameters;
        }

        protected OracleDynamicParameters ObjectToParameters(Dictionary<string, dynamic> param)
        {
            var parameters = new OracleDynamicParameters();
            if (param == null)
                return parameters;

            foreach (var key in param.Keys)
            {
                var value = param[key];
                string paramKey = $"pv_{ToSnakeCase(key)}";
                if (value.GetType() == typeof(JsonElement))
                {
                    var paramValue = (JsonElement)value;
                    var type = paramValue.ValueKind;
                    switch (type)
                    {
                        case JsonValueKind.Undefined:
                            parameters.Add(paramKey, paramValue, OracleMappingType.NVarchar2);
                            break;
                        case JsonValueKind.Object:
                            parameters.Add(paramKey, paramValue.GetRawText(), OracleMappingType.NVarchar2);
                            break;
                        case JsonValueKind.Array:
                            parameters.Add(paramKey, paramValue.GetRawText(), OracleMappingType.NVarchar2);
                            break;
                        case JsonValueKind.String:
                            parameters.Add(paramKey, paramValue.GetString(), OracleMappingType.NVarchar2);
                            break;
                        case JsonValueKind.Number:
                            parameters.Add(paramKey, paramValue.GetDouble(), OracleMappingType.Double);
                            break;
                        case JsonValueKind.True:
                            parameters.Add(paramKey, paramValue.GetString(), OracleMappingType.NVarchar2);
                            break;
                        case JsonValueKind.False:
                            parameters.Add(paramKey, paramValue.GetString(), OracleMappingType.NVarchar2);
                            break;
                        case JsonValueKind.Null:
                            parameters.Add(paramKey, null, OracleMappingType.NVarchar2);
                            break;
                        default:
                            parameters.Add(paramKey, paramValue.GetString(), OracleMappingType.NVarchar2);
                            break;
                    }
                }
                else
                {
                    parameters.Add(paramKey, value);
                }
            }
            return parameters;
        }

        private string ToPascalCaseString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var str_new = str.ToLower().Replace("_", " ");
                TextInfo info = CultureInfo.CurrentCulture.TextInfo;
                string titleCase = info.ToTitleCase(str_new).Replace(" ", string.Empty);
                return string.Join(char.ToLower(titleCase[0]), "", titleCase.Substring(1));
            }
            return str;
        }

        private string ToSnakeCase(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.Length < 2)
            {
                return text;
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));
            for (int i = 1; i < text.Length; ++i)
            {
                char c = text[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public DataSet ConvertDataReaderToDataSet(IDataReader data)
        {
            DataSet ds = new DataSet();
            int i = 0;
            while (!data.IsClosed)
            {
                ds.Tables.Add("Table" + (i + 1));
                ds.EnforceConstraints = false;
                ds.Tables[i].Load(data);
                i++;
            }
            return ds;
        }

        public List<Dictionary<string, object>> DataReaderToDynamicData(IDataReader reader)
        {
            var rows = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                if (!reader.IsClosed)
                {
                    var dict = new Dictionary<string, object>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.GetValue(i);
                        Type valueType = value.GetType();
                        if (valueType.Name.ToLower() == "decimal")
                        {
                            decimal decimalValue = reader.GetDecimal(i);
                            if (decimalValue.IsInteger())
                            {
                                value = reader.GetInt32(i);
                            }
                            else
                            {
                                value = decimalValue;
                            }
                        }
                        dict[ToPascalCaseString(reader.GetName(i))] = value;
                    }
                    rows.Add(dict);
                }
            }
            return rows;
        }

        public static Exception ThrowOracleUserException(OracleException ex, string commandText = "")
        {
            //Logger.LogError(ex, "CommandText: {}", commandText);
            if (ex.Number == ORACLE_WRONG_PARAMETERS)
            {
                return CreateErrorWithSubMessage(ERR_SQL_ASSIGN_PARAMS_FAIL, ex.Message);
            }

            if (ex.Number == ORACLE_USER_HANDLED_EXCEPTION_CODE)
            {
                var match = Regex.Match(ex.Message, "<ERROR ID=([-0-9]+)>([^<]*)</ERROR>");
                if (match.Success)
                {
                    var errCode = int.Parse(match.Groups[1].Value);
                    var errMessage = match.Groups[2].Value;

                    if (!string.IsNullOrEmpty(errMessage))
                    {
                        return CreateErrorWithSubMessage(errCode, errMessage);
                    }
                    return CreateError(errCode);
                }
            }
            return CreateErrorWithSubMessage(ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
        }

        public static FaultException CreateError(int errorCode)
        {
            var ex = new FaultException(new FaultReason(""), new FaultCode(errorCode.ToString()), "");
            return ex;
        }

        public static Exception CreateErrorWithSubMessage(int errorCode, string errorData)
        {
            var ex = new FaultException(new FaultReason(errorData), new FaultCode(errorCode.ToString()), "");
            return ex;
        }
    }
}
