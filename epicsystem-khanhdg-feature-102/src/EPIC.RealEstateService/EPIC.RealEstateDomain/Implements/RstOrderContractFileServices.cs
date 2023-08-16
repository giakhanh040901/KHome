using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.FileEntities.Settings;
using EPIC.FillContractData.Dto;
using EPIC.FillContractFile.Services;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using EPIC.RealEstateEntities.Dto.RstOrderContractFile;
using EPIC.RealEstateRepositories;
using EPIC.RealEstateSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.EnumType;
using EPIC.Utils.Filter;
using EPIC.Utils.SharedApiService;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstOrderContractFileServices : FillDataContractFileBaseServices
    {
        private readonly IMapper _mapper;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstOrderSellingPolicyEFRepository _rstOrderSellingPolicyEFRepository;
        private readonly RstContractTemplateTempEFRepository _rstContractTemplateTempEFRepository;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstDistributionContractTemplateEFRepository _rstDistributionContractTemplateEFRepository;
        private readonly RstDistributionProductItemEFRepository _rstDistributionProductItemEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstOrderContractFileEFRepository _rstOrderContractFileEFRepository;
        private readonly RstOpenSellContractTemplateEFRepository _rstOpenSellContractTemplateEFRepository;
        private readonly RstDistributionPolicyEFRepository _rstDistributionPolicyEFRepository;
        private readonly RstHistoryUpdateEFRepository _rstHistoryUpdateEFRepository;
        private readonly IRstContractCodeServices _rstContractCodeServices;

        //Services

        public RstOrderContractFileServices(EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<RstOrderContractFileServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            SharedMediaApiUtils sharedMediaApiUtils,
            SharedSignServerApiUtils sharedSignServerApiUtils,
            IHttpContextAccessor httpContext,
            IOptions<FileConfig> fileConfig,
            IOptions<UrlConfirmReceiveContract> urlConfirmReceiveContract,
            IRstContractCodeServices rstContractCodeServices
            )
            : base(dbContext, logger, configuration, databaseOptions, sharedMediaApiUtils, sharedSignServerApiUtils, httpContext, fileConfig, urlConfirmReceiveContract)
        {
            _mapper = mapper;
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstDistributionContractTemplateEFRepository = new RstDistributionContractTemplateEFRepository(dbContext, logger);
            _rstOrderSellingPolicyEFRepository = new RstOrderSellingPolicyEFRepository(dbContext, logger);
            _rstContractTemplateTempEFRepository = new RstContractTemplateTempEFRepository(dbContext, logger);
            _rstDistributionProductItemEFRepository = new RstDistributionProductItemEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstOrderContractFileEFRepository = new RstOrderContractFileEFRepository(dbContext, logger);
            _rstOpenSellContractTemplateEFRepository = new RstOpenSellContractTemplateEFRepository(dbContext, logger);
            _rstDistributionPolicyEFRepository = new RstDistributionPolicyEFRepository(dbContext, logger);
            _rstHistoryUpdateEFRepository = new RstHistoryUpdateEFRepository(dbContext, logger);

            //Services
            _rstContractCodeServices = rstContractCodeServices;
        }

        public override List<ReplaceTextDto> GetReplaceTextContractFile(ContractFileInputDtoBase input)
        {
            var newInput = input as RstExportContracDto;
            if (newInput == null)
            {
                throw new InvalidCastException($"Đầu vào không thuộc loại {nameof(RstReplaceTextContractFileDto)}");
            }
            var replateText = base.GetReplaceTextCustomer(input);
            var project = _rstProjectEFRepository.FindById(newInput.ProjectId).ThrowIfNull(_dbContext, Utils.ErrorCode.RstProjectNotFound);
            var produtItem = _rstProductItemEFRepository.FindById(newInput.ProductItemId).ThrowIfNull(_dbContext, Utils.ErrorCode.RstProductItemNotFound);

            var openSellDeital = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(newInput.OpenSellDetailId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);

            var depositValue = _rstProductItemEFRepository.ProductItemPriceByDistribution((decimal)produtItem.Price, openSellDeital.DistributionId);

            replateText.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, input.ContractCode),
                new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, input.ContractCode),
            });
            replateText.AddRange(new List<ReplaceTextDto>()
            {
                #region Dự án
                new ReplaceTextDto(RstPropertiesContractFile.RST_PROJECT_NAME, project.Name.ToUpper()),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PROJECT_ADDRESS, project.Address),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PROJECT_LAND_AREA, project.LandArea),
                #endregion
                
                #region Căn hộ
                new ReplaceTextDto(RstPropertiesContractFile.RST_PRODUCT_ITEM_CODE, produtItem.Code),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PRODUCT_ITEM_LAND_AREA, produtItem.LandArea.ToString()),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PRODUCT_ITEM_CLASSIFY_TYPE, RstClassifyType.GetClassifyTypeText(produtItem.ClassifyType)),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PRODUCT_ITEM_CARPET_AREA, produtItem.CarpetArea.ToString()),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PRODUCT_ITEM_BUILT_UP_AREA, produtItem.BuiltUpArea.ToString()),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PRODUCT_ITEM_PRICE, (double?) produtItem.Price,  EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PRODUCT_ITEM_PRICE_TEXT, (double?) produtItem.Price,  EnumReplaceTextFormat.NumberToWord),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PRODUCT_ITEM_UNIT_PRICE, (double?) produtItem.UnitPrice,  EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(RstPropertiesContractFile.RST_PRODUCT_ITEM_UNIT_PRICE_TEXT, (double?) produtItem.UnitPrice, EnumReplaceTextFormat.NumberToWord),
                new ReplaceTextDto(RstPropertiesContractFile.RST_DEPOSIT_AMOUNT, (double?) depositValue.DepositPrice, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(RstPropertiesContractFile.RST_DEPOSIT_AMOUNT_TEXT, (double?) depositValue.DepositPrice, EnumReplaceTextFormat.NumberToWord),
                #endregion

                #region Khác

                #endregion
            });
            return replateText;
        }

        public override Task<ExportResultDto> ExportFileContract(ExportContractInputDtoBase input, string contractTemplateTempUrl = null)
        {
            var newInput = input as RstExportContracDto;
            if (newInput == null)
            {
                throw new InvalidCastException($"Đầu vào không thuộc loại {nameof(RstReplaceTextContractFileDto)}");
            }

            var contractTemplateTemp = _rstContractTemplateTempEFRepository.FindById(newInput.ContractTemplateTempId).ThrowIfNull(_dbContext, ErrorCode.RstContractTemplateTempNotFound);
            return base.ExportFileContract(newInput, contractTemplateTemp.FileInvestor);
        }
        public override Task<SaveFileDto> SaveContract(SaveContractInputBaseDto input, string contractTemplateUrl = null)
        {
            var contractTemplate = _rstContractTemplateTempEFRepository.FindByIdForUpdateContractFile(input.ContractTemplateId, input.ContractType);
            if (contractTemplate == null || contractTemplate?.ContractTemplateUrl == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.RstContractTemplateTempNotFound);
            }
            return base.SaveContract(input, contractTemplate.ContractTemplateUrl);
        }

        /// <summary>
        /// Find All Contract Template cho App
        /// </summary>
        /// <param name="openSellDetailId"></param>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public List<RstOpenSellContractTemplateDto> FindAllForApp(int openSellDetailId, int? contractType = null)
        {
            _logger.LogInformation($"{nameof(FindAllForApp)}: openSellDetailId = {openSellDetailId}, contractType = {contractType}");

            List<RstOpenSellContractTemplateDto> contractTemplates = new();

            var contractTemplate = _rstContractTemplateTempEFRepository.GetAllForApp(openSellDetailId, contractType);

            return contractTemplate;
        }

        /// <summary>
        /// Sinh hợp đồng mẫu
        /// </summary>
        /// <param name="order"></param>
        /// <param name="data"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public async Task CreateContractFileByOrder(RstOrder order, List<ReplaceTextDto> data, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(CreateContractFileByOrder)}: order = {JsonSerializer.Serialize(order)}");
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            var productItem = _rstProductItemEFRepository.FindById(order.ProductItemId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            var distributionPolicy = _rstDistributionPolicyEFRepository.FindById(order.DistributionPolicyId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionPolicyNotFound);
            var openSellDetail = _rstOpenSellDetailEFRepository.FindById(order.OpenSellDetailId, order.TradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var openSell = _rstOpenSellEFRepository.FindById(openSellDetail.OpenSellId, order.TradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
            //Lấy thông tin bán theo kỳ hạn
            var project = _rstProjectEFRepository.FindById(openSell.ProjectId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            string customerType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            var businesscustomer = _businessCustomerEFRepository.FindById(cifCode.BusinessCustomerId ?? 0);
            if (cifCode.InvestorId != null)
            {
                var investorId = _investorEFRepository.GetIdentificationById(order.InvestorIdenId ?? 0);
                customerType = SharedContractTemplateType.INVESTOR;
            }
            //Lấy ra danh sách mẫu hợp đồng khi đặt lệnh
            var contractTemplates = _rstOpenSellContractTemplateEFRepository.FindAllContractTemplateTemp(openSell.Id, customerType, tradingProviderId, RstContractTypes.HD_DAT_COC, null, order.Source, Status.ACTIVE);
            if (contractTemplates.Count < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng đặt lệnh dành cho nhà đầu tư là {GetNameINVType(customerType)}"), new FaultCode(((int)ErrorCode.RstContractTemplateTempNotFound).ToString()), "");
            }
            //List<Task> taskHandleFiles = new();
            List<string> filePathOldRemove = new();
            foreach (var contract in contractTemplates)
            {
                var saveFileApp = new SaveFileDto();
                var contractCode = _rstContractCodeServices.GetContractCode(new RealEstateEntities.Dto.RstGenContractCode.RstGenContractCodeDto()
                {
                    Order = order,
                    ProductItem = productItem,
                    Project = project,
                    DistributionPolicy = distributionPolicy
                });

                data.AddRange(new List<ReplaceTextDto>(){
                    new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                    new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                });
                //Fill hợp đồng và lưu trữ
                saveFileApp = await SaveContract(new SaveContractInputBaseDto()
                {
                    ContractTemplateId = contract.ContractTemplateTempId,
                    ContractType = customerType,
                    TradingProviderId = tradingProviderId,
                    ReplaceTexts = data,
                });
                //Lưu đường dẫn vào bảng Order Contract File
                var orderContractFile = new RstOrderContractFile
                {
                    ContractTempId = contract.ContractTemplateTempId,
                    FileTempUrl = saveFileApp.FileTempUrl,
                    FileTempPdfUrl = saveFileApp?.FileSignatureUrl,
                    OrderId = order.Id,
                    TradingProviderId = tradingProviderId,
                    PageSign = saveFileApp?.PageSign ?? 1,
                    ContractCodeGen = contractCode
                };
                _rstOrderContractFileEFRepository.Add(orderContractFile, userName, tradingProviderId);
            }
            _dbContext.SaveChanges();
        }

        [Queue(HangfireQueues.RealEstate)]
        [HangfireLogEverything]
        public async Task CreateContractFileOrderApp(RstOrder order, List<ReplaceTextDto> data, int tradingProviderId)
        {
            var transaction = _dbContext.Database.BeginTransaction();
            await CreateContractFileByOrder(order, data, tradingProviderId);
            transaction.Commit();
        }

        public override ExportResultDto ExportContract(ExportOrderContractFileDto input)
        {
            var newInput = input as RstExportOrderContractFileDto;
            _logger.LogInformation($"{nameof(ExportContract)}: input = {JsonSerializer.Serialize(input)}");
            var orderContractFile = _rstOrderContractFileEFRepository.FindById(newInput.Id);
            return base.ExportContract(new ExportOrderContractFileDto()
            {
                FileScanUrl = orderContractFile.FileScanUrl,
                FileTempPdfUrl = orderContractFile.FileTempPdfUrl,
                FileTempUrl = orderContractFile.FileTempUrl,
                ContractType = newInput.ContractType
            });
        }

        /// <summary>
        /// Tải file pdf
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        public ExportResultDto ExportContractTempPdf(int orderContractFileId)
        {
            _logger.LogInformation($"{nameof(ExportContract)}: orderContractFileId = {orderContractFileId}");
            var result = new ExportResultDto();
            var orderContractFile = _rstOrderContractFileEFRepository.FindById(orderContractFileId);

            if (orderContractFile == null)
            {
                throw new FaultException(new FaultReason($"File pdf bị sinh lỗi: {orderContractFileId}"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
            }
            else if (orderContractFile?.FileTempPdfUrl == null)
            {
                throw new FaultException(new FaultReason($"File pdf bị sinh lỗi: {orderContractFileId}"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
            }

            var url = orderContractFile.FileScanUrl ?? orderContractFile.FileTempPdfUrl;
            var path = ContractDataUtils.GetParams(url);

            var folder = path["folder"];
            var fileName = path["file"];
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                _logger.LogError($"FileTempPdfUrl không tồn tại: fullPath = {fullPath}, orderContractFileId = {orderContractFileId}");
                throw new FaultException(new FaultReason($"File hợp đồng mẫu không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            result.fileDownloadName = fileName;
            return result;
        }

        /// <summary>
        /// Get danh sách file hợp đồng của order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<RstOrderContractFileDto> GetAllFileTempPdfOrder(int orderId)
        {
            _logger.LogInformation($"{nameof(ExportContract)}: orderId = {orderId}");
            var orderContractFiles = _rstOrderContractFileEFRepository.FindAll(orderId);
            var result = _mapper.Map<List<RstOrderContractFileDto>>(orderContractFiles);
            foreach (var item in result)
            {
                var contractTemp = _rstContractTemplateTempEFRepository.FindById(item.ContractTempId).ThrowIfNull(_dbContext, ErrorCode.RstContractTemplateTempNotFound);
                item.ContractTemplateTempName = contractTemp.Name;
            }
            return result;
        }

        /// <summary>
        /// Update file scan hợp đồng
        /// </summary>
        /// <param name="input"></param>
        public void UpdateFileScanContract(RstUpdateOrderContractFileDto input)
        {
            _logger.LogInformation($"{nameof(UpdateFileScanContract)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderContract = _rstOrderContractFileEFRepository.FindById(input.Id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderContractFileNotFound);
            orderContract.FileScanUrl = input.FileScanUrl ?? orderContract.FileScanUrl;

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate()
            {
                RealTableId = orderContract.OrderId,
                FieldName = RstFieldName.UPDATE_ORDER_UPLOAD_FILE_SCAN,
                UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                Action = ActionTypes.CAP_NHAT,
                Summary = RstHistoryUpdateSummary.SUMMARY_UPDATE_FILE_SCAN,
                CreatedDate = DateTime.Now,
            }, username);

            _rstOrderContractFileEFRepository.Update(orderContract, username, tradingProviderId);
            _dbContext.SaveChanges();
        }


        public async Task UpdateContractFile(int orderId)
        {
            _logger.LogInformation($"{nameof(UpdateContractFile)}: orderId = {orderId}");
            string filePath = _fileConfig.Value.Path;
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var order = _rstOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var productItem = _rstProductItemEFRepository.FindById(order.ProductItemId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            var contractTemplates = _rstContractTemplateTempEFRepository.FindAllForUpdateContractFile(order.OpenSellDetailId, contractTemplateType, tradingProviderId, null, null, order.Source, Status.ACTIVE).ThrowIfNull(_dbContext, ErrorCode.GarnerContractTemplateNotFound);
            if (contractTemplates.Count < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng dành cho nhà đầu tư là {GetNameINVType(contractTemplateType)}"), new FaultCode(((int)ErrorCode.GarnerContractTemplateNotFound).ToString()), "");
            }
            foreach (var contractTemplate in contractTemplates)
            {
                //lấy data để fill hợp đồng
                var data = GetReplaceTextContractFile(new RstExportContracDto()
                {
                    ProductItemId = order.ProductItemId,
                    ProjectId = productItem.ProjectId,
                    OpenSellDetailId = order.OpenSellDetailId,
                    InvestorId = cifCode.InvestorId,
                    BusinessCustomerId = cifCode.BusinessCustomerId,
                    IdentificationId = order.InvestorIdenId ?? 0,
                    TradingProviderId = order.TradingProviderId,
                    ConfigContractCodeId = contractTemplate.ConfigContractId
                });
                //Lấy ra danh sách hợp đồng theo order
                var contractFiles = _rstOrderContractFileEFRepository.FindByOrderAndContractTemplate(order.Id, contractTemplate.Id, tradingProviderId);
                var saveFileApp = new SaveFileDto();
                if (contractFiles.Count > 0)
                {
                    //update các hợp đồng đã có (đặt lệnh và rút tiền)
                    foreach (var contract in contractFiles)
                    {
                        if (contract.IsSign == IsSignPdf.No || contract.IsSign == null)
                        {
                            //Fill hợp đồng và lưu trữ
                            saveFileApp = await SaveContract(new SaveContractInputBaseDto()
                            {
                                ContractTemplateId = contract.ContractTempId,
                                ContractType = contractTemplateType,
                                TradingProviderId = tradingProviderId,
                                ReplaceTexts = data,
                            });
                            FileUtils.RemoveOrderContractFile(new OrderContractFileRemoveDto
                            {
                                FileScanUrl = contract.FileScanUrl,
                                FileTempPdfUrl = contract.FileTempPdfUrl,
                                FileSignatureUrl = contract.FileSignatureUrl,
                                FileTempUrl = contract.FileTempUrl,
                            }, filePath);
                            //Lưu đường dẫn vào bảng Disrtibution Contract  
                            contract.FileTempUrl = saveFileApp.FileTempUrl;
                            contract.FileSignatureUrl = saveFileApp?.FileSignatureUrl ?? contract.FileSignatureUrl;
                            contract.FileTempPdfUrl = saveFileApp?.FileSignatureUrl ?? contract.FileTempPdfUrl;
                            contract.PageSign = saveFileApp?.PageSign ?? contract.PageSign;
                            contract.Deleted = YesNo.NO;
                            _rstOrderContractFileEFRepository.Update(contract, userName, tradingProviderId);
                        }
                    }
                }
                else
                {
                    //Fill hợp đồng và lưu trữ
                    saveFileApp = await SaveContract(new SaveContractInputBaseDto()
                    {
                        ContractTemplateId = contractTemplate.ContractTemplateTempId,
                        ContractType = contractTemplateType,
                        TradingProviderId = tradingProviderId,
                        ReplaceTexts = data,
                    });
                    //Lưu đường dẫn vào bảng order contract file
                    var orderContractFile = new RstOrderContractFile
                    {
                        ContractTempId = contractTemplate.Id,
                        FileTempUrl = saveFileApp?.FileTempUrl,
                        FileSignatureUrl = saveFileApp?.FileSignatureUrl,
                        FileTempPdfUrl = saveFileApp?.FileSignatureUrl,
                        OrderId = order.Id,
                        TradingProviderId = tradingProviderId,
                        PageSign = saveFileApp?.PageSign ?? 1
                    };
                    _rstOrderContractFileEFRepository.Add(orderContractFile, userName, tradingProviderId);
                }
            }
            _dbContext.SaveChanges();

        }
    }
}
