using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.GuaranteeAsset;
using EPIC.Entities.Dto.GuaranteeFile;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondGuaranteeAssetRepository 
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string ADD_EP_GUARANTEE_ASSET_PROC = "PKG_GUARANTEE_ASSET.PROC_GUARANTEE_ASSET_ADD";
        private static string DELETE_EP_GUARANTEE_ASSET_PROC = "PKG_GUARANTEE_ASSET.PROC_GUARANTEE_ASSET_DELETE";
        private static string UPDATE_EP_GUARANTEE_ASSET_PROC = "PKG_GUARANTEE_ASSET.PROC_GUARANTEE_ASSET_UPDATE";
        private static string GET_EP_GUARANTEE_ASSET_PROC = "PKG_GUARANTEE_ASSET.PROC_GUARANTEE_ASSET_GET";
        private static string GET_ALL_EP_GUARANTEE_ASSET_PROC = "PKG_GUARANTEE_ASSET.PROC_GUARANTEE_ASSET_GET_ALL";
        private static string ADD_EP_GUARANTEE_FILE_PROC = "PKG_GUARANTEE_FILE.PROC_GUARANTEE_FILE_ADD";
        private static string DELETE_EP_GUARANTEE_FILE_PROC = "PKG_GUARANTEE_FILE.PROC_GUARANTEE_FILE_DELETE";
        private static string UPDATE_EP_GUARANTEE_FILE_PROC = "PKG_GUARANTEE_FILE.PROC_GUARANTEE_FILE_UPDATE";
        private static string GET_EP_GUARANTEE_FILE_PROC = "PKG_GUARANTEE_FILE.PROC_GUARANTEE_FILE_GET";
        private static string GET_ALL_EP_GUARANTEE_FILE_PROC = "PKG_GUARANTEE_FILE.PROC_GUARANTEE_FILE_GET_ALL";


        public BondGuaranteeAssetRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }
        public BondGuaranteeAsset Add(BondGuaranteeAsset entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondGuaranteeAsset>(ADD_EP_GUARANTEE_ASSET_PROC, new
            {
                pv_PRODUCT_BOND_ID = entity.BondId,
                pv_CODE = entity.Code,
                pv_ASSET_VALUE = entity.AssetValue,
                pv_DESCRIPTION_ASSET = entity.DescriptionAsset,
                SESSION_USERNAME = entity.CreatedBy,
            });
        }



        public int Delete(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(DELETE_EP_GUARANTEE_ASSET_PROC, new
            {
                pv_GUARANTEE_ASSET_ID = id
            });
        }

        public int DeleteFile(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(DELETE_EP_GUARANTEE_FILE_PROC, new
            {
                pv_GUARANTEE_FILE_ID = id
            });
        }

        public void Update(BondGuaranteeAsset entity)
        {
             _oracleHelper.ExecuteProcedureNonQuery(UPDATE_EP_GUARANTEE_ASSET_PROC, new
            {
                pv_GUARANTEE_ASSET_ID = entity.Id,
                pv_PRODUCT_BOND_ID = entity.BondId,
                pv_CODE = entity.Code,
                pv_ASSET_VALUE = entity.AssetValue,
                pv_DESCRIPTION_ASSET = entity.DescriptionAsset,
                SESSION_USERNAME = entity.ModifiedBy,
            });
        }

        public BondGuaranteeAsset FindById(int id)
        {
            BondGuaranteeAsset guaranteeAsset = _oracleHelper.ExecuteProcedureToFirst<BondGuaranteeAsset>(GET_EP_GUARANTEE_ASSET_PROC, new
            {
                pv_GUARANTEE_ASSET_ID = id,
            });
            return guaranteeAsset;
        }

        public IEnumerable<BondGuaranteeFile> FindAllByIdFile(int id)
        {
            var guaranteeFile = _oracleHelper.ExecuteProcedurePaging<BondGuaranteeFile>(GET_ALL_EP_GUARANTEE_FILE_PROC, new
            {
                pv_GUARANTEE_ASSET_ID = id,
                PAGE_SIZE = -1,
                PAGE_NUMBER = 0,
                KEY_WORD = "",
            });
            return guaranteeFile.Items;
        }


        public PagingResult<BondGuaranteeFile> FindAllGuaranteeFile(int guaranteeAssetId, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BondGuaranteeFile>(GET_ALL_EP_GUARANTEE_FILE_PROC, new
            {
                pv_GUARANTEE_ASSET_ID = guaranteeAssetId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public BondGuaranteeFile AddFile(BondGuaranteeFile entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondGuaranteeFile>(ADD_EP_GUARANTEE_FILE_PROC, new
            {
                pv_GUARANTEE_ASSET_ID = entity.GuaranteeAssetId,
                pv_TITLE = entity.Title,
                pv_FILE_URL = entity.FileUrl,
                SESSION_USERNAME = entity.CreatedBy,
            });
        }

        public int DeleteGuaranteeFile(int id)
        {
            _logger.LogInformation($"Delete Guaramtee asset: {id}");
            return _oracleHelper.ExecuteProcedureNonQuery(DELETE_EP_GUARANTEE_FILE_PROC, new
            {
                pv_GUARANTEE_FILE_ID = id
            });
        }

        public int UpdateFile(BondGuaranteeFile entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_EP_GUARANTEE_FILE_PROC, new
            {
                pv_GUARANTEE_FILE_ID = entity.Id,
                pv_GUARANTEE_ASSET_ID = entity.GuaranteeAssetId,
                pv_TITLE = entity.Title,
                pv_FILE_URL = entity.FileUrl
            });
            return result;
        }

        public GuaranteeFileDto FindGuaranteeFileById(int id)
        {
            var guaranteeFile = _oracleHelper.ExecuteProcedureToFirst<GuaranteeFileDto>(GET_EP_GUARANTEE_FILE_PROC, new
            {
                pv_GUARANTEE_FILE_ID = id,
            });
            return guaranteeFile;
        }

        public PagingResult<GuaranteeAssetDto> FindAll(int productBondId, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<GuaranteeAssetDto>(GET_ALL_EP_GUARANTEE_ASSET_PROC, new
            {
                pv_PRODUCT_BOND_ID = productBondId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public List<BondGuaranteeAsset> Filter(Func<Predicate<BondGuaranteeAsset>, bool> expression)
        {
            throw new NotImplementedException();
        }
    }
}
