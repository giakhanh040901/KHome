using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.Sale;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.CoreRepositories
{
    public class DepartmentRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private const string PROC_DEPARTMENT_GET_ALL = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_GET_ALL";
        private const string PROC_DEPARTMENT_ADD = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_ADD";
        private const string PROC_DEPARTMENT_UPDATE = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_UPDATE";
        private const string PROC_DEPARTMENT_MOVE_SALE = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_MOVE_SALE";
        private const string PROC_DEPARTMENT_ASSIGN_MANAGER = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_ASSIGN_MANAGER";
        private const string PROC_DEPARTMENT_ASSIGN_MANAGER2 = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_ASSIGN_MANAGER2";
        private const string PROC_DEPARTMENT_ALL_LIST_CHILD = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_ALL_LIST_CHILD";
        private const string PROC_DEPARTMENT_ALL_LIST_SALE = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_ALL_LIST_SALE";
        private const string PROC_DEPARTMENT_GET_BY_ID = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_GET_BY_ID";
        private const string PROC_DEPARTMENT_GET_BY_SALE_ID = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_GET_BY_SALE_ID";
        private const string PROC_DEPARTMENT_DELETE = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_DELETE";
        private const string PROC_DEPARTMENT_DELETED_MANAGER = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_DELETED_MANAGER";
        private const string PROC_DEPARTMENT_CHANGE_LIST_SALE = "PKG_CORE_DEPARTMENT.PROC_DEPARTMENT_CHANGE_LIST_SALE";
        private const string PROC_UPDATE_LIST_SALE_TO_NEW_DEPARTMENT = "PKG_CORE_DEPARTMENT.PROC_UPDATE_LIST_SALE_TO_NEW_DEPARTMENT";
        private const string PROC_UPDATE_LEVEL_DEPARTMENT = "PKG_CORE_DEPARTMENT.PROC_UPDATE_LEVEL_DEPARTMENT";
        private const string PROC_DEPARTMENT_ALL_LIST_PARENT = "PKG_CORE_EXCEL_REPORT.PROC_DEPARTMENT_ALL_LIST_PARENT";

        public DepartmentRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public Department Add(Department input)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Department>(PROC_DEPARTMENT_ADD, new
            {
                pv_DEPARTMENT_NAME = input.DepartmentName,
                pv_DEPARTMENT_ADDRESS = input.DepartmentAddress,
                pv_PARENT_ID = input.ParentId,
                pv_CREATED_BY = input.CreatedBy,
                pv_TRADING_PROVIDER_ID = input.TradingProviderId
            });
        }

        public int MoveSale(MoveSaleDto input, string createdBy, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DEPARTMENT_MOVE_SALE, new
            {
                pv_SALE_ID = input.SaleId,
                pv_SALE_TYPE = input.SaleType,
                pv_DEPARTMENT_ID = input.DepartmentId,
                pv_CREATED_BY = createdBy,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public int AssignManager(AssignManagerDto input, string modifiedBy, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DEPARTMENT_ASSIGN_MANAGER, new
            {
                pv_SALE_ID = input.SaleId,
                pv_DEPARTMENT_ID = input.DepartmentId,
                pv_MODIFIED_BY = modifiedBy,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        /// <summary>
        /// Gán saler là quản lý 2 của phòng ban (quản lý này chỉ là con người)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public int AssignManager2(AssignManagerDto input, string modifiedBy, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DEPARTMENT_ASSIGN_MANAGER2, new
            {
                pv_SALE_ID = input.SaleId,
                pv_DEPARTMENT_ID = input.DepartmentId,
                pv_MODIFIED_BY = modifiedBy,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public IEnumerable<Department> FindAllListChild(int? parentId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<Department>(PROC_DEPARTMENT_ALL_LIST_CHILD, new
            {
                pv_PARENT_ID = parentId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public PagingResult<DepartmentSaleDto> FindAllListSale(int pageSize, int pageNumber, string keyword, string saleTypes, int departmentId, int tradingProviderId, bool? isInvestor)
        {
            return _oracleHelper.ExecuteProcedurePaging<DepartmentSaleDto>(PROC_DEPARTMENT_ALL_LIST_SALE, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_SALE_TYPE = saleTypes,
                pv_DEPARTMENT_ID = departmentId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_INVESTOR = isInvestor != null ? (isInvestor.Value ? YesNo.YES : YesNo.NO) : null
            });
        }

        public Department Update(Department input)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Department>(PROC_DEPARTMENT_UPDATE, new
            {
                pv_DEPARTMENT_ID = input.DepartmentId,
                pv_DEPARTMENT_NAME = input.DepartmentName,
                pv_DEPARTMENT_ADDRESS = input.DepartmentAddress,
                pv_MODIFIED_BY = input.ModifiedBy,
                pv_TRADING_PROVIDER_ID = input.TradingProviderId
            });
        }

        public PagingResult<DepartmentDto> FindAll(int pageSize, int pageNumber, string keyword, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<DepartmentDto>(PROC_DEPARTMENT_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public Department FindById(int departmentId, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Department>(PROC_DEPARTMENT_GET_BY_ID, new
            {
                pv_DEPARTMENT_ID = departmentId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public Department FindBySaleId(int saleId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Department>(PROC_DEPARTMENT_GET_BY_SALE_ID, new
            {
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        /// <summary>
        /// Xoá phòng ban nếu phòng ban trống
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns> 
        public int Delete(int departmentId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DEPARTMENT_DELETE, new
            {
                pv_DEPARTMENT_ID = departmentId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public int DeleteDepartmentManager(int departmentId, int tradingProviderId, int managerType, string ModifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DEPARTMENT_DELETED_MANAGER, new
            {
                pv_MANAGER_TYPE = managerType,
                pv_DEPARTMENT_ID = departmentId,
                pv_MODIFIED_BY = ModifiedBy,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public int DepartmentChangeListSale(int departmentId,int newDepartmentId, string modifiedBy, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DEPARTMENT_CHANGE_LIST_SALE, new
            {
                pv_DEPARTMENT_ID = departmentId,
                pv_NEW_DEPARTMENT_ID = newDepartmentId,
                SESSION_USERNAME = modifiedBy,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public int UpdateListSaleToNewDepartment(int tradingProviderId, UpdateListSaleToNewDepartment input, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_LIST_SALE_TO_NEW_DEPARTMENT, new
            {
                pv_DEPARTMENT_ID = input.DepartmentId,
                pv_NEW_DEPARTMENT_ID = input.NewDepartmentId,
                pv_LIST_SALE_IDS = input.Sales != null ? string.Join(',', input.Sales) : null,
                SESSION_USERNAME = modifiedBy,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public int UpdateLevelDepartment(int tradingProviderId, int departmentId, int? parentId, int departmentLevel, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_LEVEL_DEPARTMENT, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_DEPARTMENT_ID = departmentId,
                pv_DEPARTMENT_PARENT_ID = parentId,
                pv_DEPARTMENT_LEVEL = departmentLevel,
                SESSION_USERNAME = modifiedBy,
            });
        }
        
        public IEnumerable<Department> FindListParentDepartment(int? tradingProviderId, int? parentId)
        {
            var result = _oracleHelper.ExecuteProcedure<Department>(PROC_DEPARTMENT_ALL_LIST_PARENT, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARENT_ID = parentId
            }).ToList();

            return result;
        }
    }
}
