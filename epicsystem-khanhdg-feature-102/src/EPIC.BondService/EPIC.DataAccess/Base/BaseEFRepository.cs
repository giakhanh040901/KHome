//using Dapper.Oracle;
//using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.ServiceModel;
using System.Text.RegularExpressions;
using EPIC.GarnerEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Utils;
using EPIC.DataAccess.Base.Entities;
using Humanizer;
using DocumentFormat.OpenXml.Wordprocessing;

namespace EPIC.DataAccess.Base
{
    public class BaseEFRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly EpicSchemaDbContext _epicSchemaDbContext;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly string _seqName;
        protected readonly ILogger _logger;

        public const int ORACLE_WRONG_PARAMETERS = 6550;
        public const int ORACLE_USER_HANDLED_EXCEPTION_CODE = 20000;

        public const int ERR_SQL_BASE = 200;
        public const int ERR_SQL_OPEN_CONNECTION_FAIL = ERR_SQL_BASE + 1;
        public const int ERR_SQL_EXECUTE_COMMAND_FAIL = ERR_SQL_BASE + 2;
        public const int ERR_SQL_DISCOVERY_PARAMS_FAIL = ERR_SQL_BASE + 3;
        public const int ERR_SQL_ASSIGN_PARAMS_FAIL = ERR_SQL_BASE + 4;

        public BaseEFRepository(DbContext dbContext, string seqName = null)
        {
            _dbContext = dbContext;
            _epicSchemaDbContext = dbContext as EpicSchemaDbContext;
            _dbSet = dbContext.Set<TEntity>();
            _seqName = seqName;
        }

        public BaseEFRepository(DbContext dbContext, ILogger logger, string seqName = null)
        {
            _dbContext = dbContext;
            _epicSchemaDbContext = dbContext as EpicSchemaDbContext;
            _dbSet = dbContext.Set<TEntity>();
            _seqName = seqName;
            _logger = logger;
        }

        public DbSet<TEntity> Entity => _dbSet;
        public IQueryable<TEntity> EntityNoTracking => _dbSet.AsNoTracking<TEntity>();

        public decimal NextKey()
        {
            if (_seqName == null)
            {
                throw new Exception($"Chưa cấu hình sequence cho repository: {this.GetType()}");
            }

            OracleParameter[] @params =
            {
                new OracleParameter("SEQ_NAME", OracleDbType.Varchar2) { Direction = ParameterDirection.Input, Value = _seqName },
                new OracleParameter("SEQ_OUT", OracleDbType.Decimal) { Direction = ParameterDirection.Output }
            };
            _dbContext.Database.ExecuteSqlRaw("BEGIN EPIC_REAL_ESTATE.GET_SEQ(:SEQ_NAME, :SEQ_OUT); END;", @params);
            var result = @params[1].Value;
            decimal valueResult = decimal.Parse(result.ToString());
            return valueResult;
        }

        public decimal NextKey(string seqName)
        {
            if (_seqName == null)
            {
                throw new Exception($"Chưa cấu hình sequence cho repository: {this.GetType()}");
            }

            OracleParameter[] @params =
            {
                new OracleParameter("SEQ_NAME", OracleDbType.Varchar2) { Direction = ParameterDirection.Input, Value = seqName },
                new OracleParameter("SEQ_OUT", OracleDbType.Decimal) { Direction = ParameterDirection.Output }
            };
            _dbContext.Database.ExecuteSqlRaw("BEGIN GET_SEQ(:SEQ_NAME, :SEQ_OUT); END;", @params);
            var result = @params[1].Value;
            decimal valueResult = decimal.Parse(result.ToString());
            return valueResult;
        }

        /// <summary>
        /// Lấy mã lỗi
        /// </summary>
        /// <param name="defError"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetDefError(int defError)
        {
            var err = _epicSchemaDbContext.DefErrors.AsNoTracking().FirstOrDefault(d => d.ErrCode == defError);
            if (err == null)
            {
                throw new Exception($"Không tìm thấy mã lỗi {defError}");
            }
            return err.ErrMessage;
        }

        /// <summary>
        /// Ném ra ngoại lệ
        /// </summary>
        /// <param name="errorCode"></param>
        /// <exception cref="FaultException"></exception>
        public void ThrowException(ErrorCode errorCode)
        {
            throw new FaultException(new FaultReason(GetDefError((int)errorCode)), new FaultCode(((int)errorCode).ToString()), "");
        }

        /// <summary>
        /// Ném ra ngoại lệ truyền message
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <exception cref="FaultException"></exception>
        public void ThrowException(string errorMessage)
        {
            throw new FaultException(new FaultReason(errorMessage), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
        }

        /// <summary>
        /// Ném ra ngoại lệ, kèm dataRef
        /// </summary>
        /// <param name="errorCode"></param>
        /// <exception cref="FaultException"></exception>
        public void ThrowException(ErrorCode errorCode, object dataRef)
        {
            throw new FaultException(new FaultReason(GetDefError((int)errorCode) + $", ref: {dataRef}"), new FaultCode(((int)errorCode).ToString()), "");
        }

        /// <summary>
        /// Log ra ngoại lệ
        /// </summary>
        /// <param name="errorCode"></param>
        public void LogError(ErrorCode errorCode)
        {
            _logger?.LogError(GetDefError((int)errorCode));
        }

        /// <summary>
        /// Log ra ngoại lệ, kèm dataRef
        /// </summary>
        /// <param name="errorCode"></param>
        /// <exception cref="FaultException"></exception>
        public void LogError(ErrorCode errorCode, object dataRef)
        {
            _logger?.LogError(GetDefError((int)errorCode) + $", ref: {dataRef}");
        }

        /// <summary>
        /// Check null
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="errorCode"></param>
        public void CheckNull(object entity, ErrorCode errorCode)
        {
            if (entity == null)
            {
                ThrowException(errorCode);
            }
        }

        #region procedure
        public ExecuteParamAndQuery ObjectToParamAndQueryPaging(string procedure, object param)
        {
            var parameters = ObjectToParameters(param);

            parameters.Add(new OracleParameter
            {
                ParameterName = "total_items",
                OracleDbType = OracleDbType.Int32,
                Direction = ParameterDirection.Output,
            });

            parameters.Add(new OracleParameter
            {
                ParameterName = "cur",
                OracleDbType = OracleDbType.RefCursor,
                Direction = ParameterDirection.Output,
                UdtTypeName = "EPIC.pkg_data.CURSOR"
            });

            var sqlParams = OracleParamToQueryString(parameters);

            string query = $"begin {procedure}({sqlParams}); end;";

            return new ExecuteParamAndQuery
            {
                SqlQuery = query,
                Parameters = parameters?.ToArray(),
            };
        }

        public ExecuteParamAndQuery ObjectToParamAndQueryList(string procedure, object param)
        {
            var parameters = ObjectToParameters(param);

            parameters.Add(new OracleParameter
            {
                ParameterName = "cur",
                OracleDbType = OracleDbType.RefCursor,
                Direction = ParameterDirection.Output,
                UdtTypeName = "EPIC.pkg_data.CURSOR"
            });

            var sqlParams = OracleParamToQueryString(parameters);

            string query = $"begin {procedure}({sqlParams}); end;";

            return new ExecuteParamAndQuery
            {
                SqlQuery = query,
                Parameters = parameters?.ToArray(),
            };
        }

        public ExecuteParamAndQuery ObjectToParamAndQuery(string procedure, object param)
        {
            var parameters = ObjectToParameters(param);

            var sqlParams = OracleParamToQueryString(parameters);

            string query = $"begin {procedure}({sqlParams}); end;";

            return new ExecuteParamAndQuery
            {
                SqlQuery = query,
                Parameters = parameters?.ToArray(),
            };
        }

        public List<OracleParameter> ObjectToParameters(object param)
        {
            var parameters = new List<OracleParameter>();
            
            if (param == null)
                return parameters;

            var properties = param.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var key = prop.Name;
                var value = prop.GetValue(param);

                var tmp = new OracleParameter
                {
                    ParameterName = key,
                    Value = value,
                    Direction = ParameterDirection.Input
                };

                parameters.Add(tmp);                
            }
            return parameters;
        }

        public string OracleParamToQueryString(List<OracleParameter> listParam)
        {
            var result = "";
            
            if (listParam == null || listParam.Count == 0)
                return result;

            var len = listParam.Count;
            for (int i = 0; i < len; i++)
            {
                var param = listParam[i];
                var key = param.ParameterName;
                _ = param.Value;

                result += $":{key}";

                if (i + 1 < len)
                {
                    result += ",";
                }
            }

            return result;
        }

        public Exception ThrowOracleUserException(OracleException ex, string commandText = "")
        {
            _logger.LogError(ex, "CommandText: {}", commandText);
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

        public FaultException CreateError(int errorCode)
        {
            var ex = new FaultException(new FaultReason(""), new FaultCode(errorCode.ToString()), "");
            return ex;
        }

        public Exception CreateErrorWithSubMessage(int errorCode, string errorData)
        {
            var ex = new FaultException(new FaultReason(errorData), new FaultCode(errorCode.ToString()), "");
            return ex;
        }

        #endregion
    }

    public static class BaseEFRepositoryExtention
    {
        /// <summary>
        /// Trả về mã lỗi nếu object null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="epicSchemaDbContext"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="FaultException"></exception>
        public static T ThrowIfNull<T>(this T obj, EpicSchemaDbContext epicSchemaDbContext, ErrorCode errorCode) where T : class
        {
            if (obj == null)
            {
                var errCode = (int)errorCode;
                var err = epicSchemaDbContext.DefErrors.AsNoTracking().FirstOrDefault(d => d.ErrCode == errCode);
                if (err == null)
                {
                    throw new Exception($"Không tìm thấy mã lỗi {(int)errorCode}");
                }
                string errorMessage = err.ErrMessage;
                throw new FaultException(new FaultReason(errorMessage), new FaultCode(((int)errorCode).ToString()), "");
            }
            return obj;
        }

        public static T ThrowIfNull<T>(this T obj, string message) where T : class
        {
            if (obj == null)
            {
                throw new FaultException(new FaultReason(message), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }
            return obj;
        }

        /// <summary>
        /// Trả về mã lỗi nếu object null, kèm refer id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="epicSchemaDbContext"></param>
        /// <param name="errorCode"></param>
        /// <param name="dataRef"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="FaultException"></exception>
        public static T ThrowIfNull<T>(this T obj, EpicSchemaDbContext epicSchemaDbContext, ErrorCode errorCode, object dataRef) where T : class
        {
            if (obj == null)
            {
                var errCode = (int)errorCode;
                var err = epicSchemaDbContext.DefErrors.AsNoTracking().FirstOrDefault(d => d.ErrCode == errCode);
                if (err == null)
                {
                    throw new Exception($"Không tìm thấy mã lỗi {(int)errorCode}");
                }
                string errorMessage = $"{err.ErrMessage}, (dataRef: {dataRef})";
                throw new FaultException(new FaultReason(errorMessage), new FaultCode(((int)errorCode).ToString()), "");
            }
            return obj;
        }

        /// <summary>
        /// Trả về mã lỗi nếu object null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public static T ThrowIfNull<T>(this T obj, ErrorCode errorCode, string errorMessage) where T : class
        {
            if (obj == null)
            {
                throw new FaultException(new FaultReason(errorMessage), new FaultCode(((int)errorCode).ToString()), "");
            }
            return obj;
        }

        /// <summary>
        /// Log error nếu null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T LogErrorIfNull<T>(this T obj, ILogger logger, string message) where T : class
        {
            if (obj == null)
            {
                logger.LogError(message);
            }
            return obj;
        }

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">Kết quả query muốn phân trang</param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static PagingResult<T> ToPaging<T>(this IQueryable<T> query, int? pageSize, int? pageNumber) where T : class
        {
            if (pageSize == -1)
            {
                return new PagingResult<T>
                {
                    Items = query,
                    TotalItems = query.Count()
                };
            }
            return new PagingResult<T>
            {
                Items = query.Skip(pageSize * (pageNumber - 1) ?? 0).Take(pageSize ?? 0),
                TotalItems = query.Count()
            };
        }
    }
}
