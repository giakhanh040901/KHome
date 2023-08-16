using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.GeneralContractor;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.Owner;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestEntities.Dto.ProjectImage;
using EPIC.InvestEntities.Dto.ProjectJuridicalFile;
using EPIC.InvestEntities.Dto.ProjectType;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.InvestDomain.Implements
{
    public class ProjectServices : IProjectServices
    {
        private readonly ILogger _logger;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly OwnerRepository _ownerRepository;
        private readonly GeneralContractorRepository _generalContractorRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly InvestApproveRepository _approveRepository;
        private readonly ProjectJuridicalFileRepository _projectJuridicalFileRepository;
        private readonly IMapper _mapper;
        private readonly ProjectImageRepository _projectImageRepository;
        private readonly ProjectTradingProviderRepository _projectTradingProviderRepository;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly InvestHistoryUpdateEFRepository _investHistoryUpdateEFRepository;
        private readonly InvestProjectTradingProviderEFRepository _investProjectTradingProviderEFRepository;
        private readonly IProjectTradingProviderServices _projectTradingProviderServices;
        private readonly InvestProjectEFRepository _investProjectEFRepository;

        public ProjectServices(
            ILogger<ProjectServices> logger, 
            EpicSchemaDbContext dbContext,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IProjectTradingProviderServices projectTradingProviderServices,
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _ownerRepository = new OwnerRepository(_connectionString, _logger);
            _generalContractorRepository = new GeneralContractorRepository(_connectionString, _logger);
            _approveRepository = new InvestApproveRepository(_connectionString, _logger);
            _projectJuridicalFileRepository = new ProjectJuridicalFileRepository(_connectionString, _logger);
            _mapper = mapper;
            _projectImageRepository = new ProjectImageRepository(_connectionString, _logger);
            _projectTradingProviderRepository = new ProjectTradingProviderRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _investHistoryUpdateEFRepository = new InvestHistoryUpdateEFRepository(_dbContext, _logger);
            _investProjectTradingProviderEFRepository = new InvestProjectTradingProviderEFRepository(_dbContext, _logger);
            _projectTradingProviderServices = projectTradingProviderServices;
            _investProjectEFRepository = new InvestProjectEFRepository(_dbContext, _logger);
        }

        public ProjectDto Add(CreateProjectDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = new ProjectDto();
            using (var transaction = new TransactionScope())
            {
                var project = _projectRepository.Add(new Project
                {
                    PartnerId = partnerId,
                    OwnerId = input.OwnerId.Value,
                    GeneralContractorId = input.GeneralContractorId.Value,
                    InvCode = input.InvCode,
                    InvName = input.InvName,
                    Content = input.Content,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    Image = input.Image,
                    IsPaymentGuarantee = input.IsPaymentGuarantee,
                    Area = input.Area,
                    Longitude = input.Longitude,
                    Latitude = input.Latitude,
                    LocationDescription = input.LocationDescription,
                    TotalInvestment = input.TotalInvestment,
                    ProjectProgress = input.ProjectProgress,
                    GuaranteeOrganization = input.GuaranteeOrganization,
                    TotalInvestmentDisplay = input.TotalInvestmentDisplay,
                    HasTotalInvestmentSub = input.HasTotalInvestmentSub,
                    CreatedBy = username
                });
                result = _mapper.Map<ProjectDto>(project);
                foreach (var projectType in input.ProjectTypes)
                {
                    if (projectType != InvestProjectTypes.NHA_RIENG && projectType != InvestProjectTypes.CAN_HO_CHUNG_CU && projectType != InvestProjectTypes.NHA_PHO_BIET_THU_DU_AN
                        && projectType != InvestProjectTypes.DAT_NEN_DU_AN && projectType != InvestProjectTypes.BIET_THU_NGHI_DUONG && projectType != InvestProjectTypes.CONDOTEL
                        && projectType != InvestProjectTypes.SHOPHOUSE && projectType != InvestProjectTypes.OFFICETEL)
                    {
                        throw new FaultException(new FaultReason($"Loại hình dự án không hợp lệ"), new FaultCode(((int)ErrorCode.InvestProjectTypeNotFound).ToString()), "");
                    }
                    _projectRepository.AddProjectType(new ProjectType
                    {
                        ProjectId = project.Id,
                        Type = projectType,
                    });
                }

                #region Thêm đại lý cho dự án
                if (input.ProjectTradingProvider != null)
                {
                    _projectTradingProviderServices.UpdateProjectTrading(project.Id, input.ProjectTradingProvider);
                }
                #endregion
                transaction.Complete();
            }
            _projectRepository.CloseConnection();
            return result;
        }

        public void Approve(InvestApproveDto input)
        {
            int userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, InvestApproveDataTypes.EP_INV_PROJECT);

                if (approve != null)
                {
                    _approveRepository.ApproveRequestStatus(new InvestApproveDto
                    {
                        Id = approve.Id,
                        ApproveNote = input.ApproveNote,
                        UserApproveId = userId
                    });
                }
                _projectRepository.ProjectApprove(input.Id);
                transaction.Complete();
            }
            _projectRepository.CloseConnection();
        }

        public void Cancel(InvestCancelDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, InvestApproveDataTypes.EP_INV_PROJECT);
                if (approve != null)
                {
                    _approveRepository.CancelRequest(new InvestCancelDto
                    {
                        Id = approve.Id,
                        CancelNote = input.CancelNote,
                    });
                }
                _projectRepository.ProjectCancel(input.Id);
                transaction.Complete();
            }
            _projectRepository.CloseConnection();
        }

        public void Check(InvestCheckDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_INFO);
                if (approve != null)
                {
                    _approveRepository.CheckRequest(new InvestCheckDto
                    {
                        Id = approve.Id,
                        UserCheckId = userid,
                    });
                }
                _projectRepository.ProjectCheck(input.Id);
                transaction.Complete();
            }
            _projectRepository.CloseConnection();
        }

        /// <summary>
        /// Đóng
        /// </summary>
        public void CloseOpen(int id)
        {
            _projectRepository.ProjectClose(id);
        }

        public int Delete(int id)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            return _projectRepository.Delete(id, partnerId);
        }

        private PagingResult<ProjectDto> FindAllBase(PagingResult<ProjectDto> projectList)
        {
            var result = new PagingResult<ProjectDto>();
            var items = new List<ProjectDto>();
            result.TotalItems = projectList.TotalItems;
            foreach (var projectItem in projectList.Items)
            {
                var project = _mapper.Map<ProjectDto>(projectItem);
                var owner = _ownerRepository.FindById(projectItem.OwnerId ?? 0);
                if (owner != null)
                {
                    project.Owner = _mapper.Map<ViewOwnerDto>(owner);

                    var businessCus = _businessCustomerRepository.FindBusinessCustomerById(project.Owner.BusinessCustomerId);
                    if (businessCus != null)
                    {
                        project.Owner.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCus);
                    }
                }

                //Nếu là đại lý có tổng mức đầu tư Sub thì sẽ hiện ra thế vào TotalInvestment
                if (projectItem.TotalInvestmentSub != null)
                {
                    project.TotalInvestment = projectItem.TotalInvestmentSub;
                }   
                
                var generalContractor = _generalContractorRepository.FindById(projectItem.GeneralContractorId ?? 0);
                if (generalContractor != null)
                {
                    project.GeneralContractor = _mapper.Map<ViewGeneralContractorDto>(generalContractor);

                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(project.GeneralContractor.BusinessCustomerId);
                    if (businessCustomer != null)
                    {
                        project.GeneralContractor.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    }
                }
                items.Add(project);
            }
            result.Items = items;
            return result;
        }

        public PagingResult<ProjectDto> FindAll(FilterInvestProjectDto input)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            int? partnerId = null;
            if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            //var projectList = _projectRepository.FindAll(pageSize, pageNumber, keyword, status, partnerId, tradingProviderId);
            var projectList = _investProjectEFRepository.FindAll(input, partnerId);
            return FindAllBase(projectList);
        }

        public PagingResult<ProjectDto> FindAllTradingProvider(int pageSize, int pageNumber, string keyword, int? status)
        {
            int? tradingProviderId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            int? partnerId = null;
            if(usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            var projectList = _projectRepository.FindAll(pageSize, pageNumber, keyword, status, partnerId, tradingProviderId);
            return FindAllBase(projectList);
        }

        public ProjectDto FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var projectFind = _projectRepository.FindById(id, partnerId);
            var result = _mapper.Map<ProjectDto>(projectFind);
            if (projectFind != null)
            {
                var projectJuridicalFile = _projectJuridicalFileRepository.FindAll(projectFind.Id, -1, 0, null);
                result.JuridicalFiles = _mapper.Map<List<ProjectJuridicalFileDto>>(projectJuridicalFile.Items);

                var projectType = _projectRepository.FindByProjectId(id);
                if (projectType != null)
                {
                    result.ProjectTypes = projectType.Select(r => r.Type).ToList();
                }

                var projectImage = _projectImageRepository.FindByProjectId(id);
                result.ProjectImages = _mapper.Map<List<ProjectImageDto>>(projectImage);
            }
            return result;
        }

        public void Request(CreateInvestRequestDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var actionType = InvestApproveAction.THEM;
                var checkIsUpdate = _approveRepository.GetOneByActual(input.Id, InvestApproveDataTypes.EP_INV_PROJECT);
                if (checkIsUpdate != null)
                {
                    actionType = InvestApproveAction.CAP_NHAT;
                }
                _approveRepository.CreateApproveRequest(new CreateInvestRequestDto
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = actionType,
                    DataType = InvestApproveDataTypes.EP_INV_PROJECT,
                    ReferId = input.Id,
                    Summary = input.Summary
                },null, partnerId);
                _projectRepository.ProjectRequest(input.Id);
                transaction.Complete();
            }
            _projectRepository.CloseConnection();
        }

        public void Update(UpdateProjectDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var projectFind = _dbContext.InvestProjects.FirstOrDefault(x => x.Id == input.Id && x.PartnerId == partnerId && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestProjectNotFound);

            // Thay đổi hạn mức
            if (input.TotalInvestment != projectFind.TotalInvestment)
            {
                // Tổng số tiền đầu tư trong dự án
                var totalAmountOfProject = _dbContext.InvOrders.Where(o => o.ProjectId == input.Id && (o.Status == OrderStatus.CHO_DUYET_HOP_DONG || o.Status == OrderStatus.DANG_DAU_TU)
                                            && o.Deleted == YesNo.NO).Sum(x => x.TotalValue);
                // Nếu để hạn mức thấp hơn số tiền đang đầu tư
                if (input.TotalInvestment < totalAmountOfProject)
                {
                    _investHistoryUpdateEFRepository.ThrowException(ErrorCode.InvestProjectTotalInvestmentCanNotUpdate);
                }
                // Bằng số tiền đang đầu tư: Hạn mức bằng 0 và không có cài hạn mức: Tắt showApp với distribution
                else if (input.TotalInvestment == totalAmountOfProject)
                {
                    // Lấy các phân phối chưa tắt showApp
                    var distributionShowAppProject = _dbContext.InvestDistributions.Where(p => p.ProjectId == input.Id && p.IsShowApp == YesNo.NO && p.Status == DistributionStatus.HUY_DUYET && p.Deleted == YesNo.NO);
                    foreach (var item in distributionShowAppProject)
                    {
                        item.IsShowApp = YesNo.NO;
                        _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(item.Id, YesNo.YES, YesNo.NO, InvestFieldName.UPDATE_DISTRIBUTION_IS_SHOW_APP, InvestHistoryUpdateTables.INV_DISTRIBUTION,
                            ActionTypes.CAP_NHAT, $"Hệ thống tắt show app khi thay đổi hạn mức của dự án", DateTime.Now), username);
                    }
                }
            }

            // Cập nhật thông tin dự án
            projectFind.InvCode = input.InvCode;
            projectFind.InvName = input.InvName;
            projectFind.Content = input.Content;
            projectFind.StartDate = input.StartDate;
            projectFind.EndDate = input.EndDate;
            projectFind.Image = input.Image;
            projectFind.IsPaymentGuarantee = input.IsPaymentGuarantee;
            projectFind.Area = input.Area;
            projectFind.Longitude = input.Longitude;
            projectFind.Latitude = input.Latitude;
            projectFind.LocationDescription = input.LocationDescription;
            projectFind.TotalInvestment = input.TotalInvestment;
            projectFind.ProjectType = input.ProjectType;
            projectFind.ProjectProgress = input.ProjectProgress;
            projectFind.GuaranteeOrganization = input.GuaranteeOrganization;
            projectFind.ModifiedBy = username;
            projectFind.TotalInvestmentDisplay = input.TotalInvestmentDisplay;
            projectFind.HasTotalInvestmentSub = input.HasTotalInvestmentSub;

            // Cập nhật thông tin phân phối cho đại lý
            if (input.ProjectTradingProvider != null)
            {
                var projectTradingProviders = _dbContext.InvestProjectTradingProviders.Where(p => p.ProjectId == projectFind.Id && p.Deleted == YesNo.NO && p.PartnerId == partnerId);
                // Xóa đi các đại lý được phân phối
                var projectTradingProviderRemove = projectTradingProviders.Where(p => !input.ProjectTradingProvider.Select(x => x.Id).Contains(p.Id));
                foreach (var item in projectTradingProviderRemove)
                {
                    // Kiểm tra xem đại lý có phát sinh thanh toán chưa
                    var checkOrder = _dbContext.InvOrders.Any(o => o.ProjectId == projectFind.Id && o.TradingProviderId == item.TradingProviderId
                                    && new List<int> { OrderStatus.CHO_DUYET_HOP_DONG, OrderStatus.DANG_DAU_TU, OrderStatus.TAT_TOAN }.Contains(o.Status) && o.Deleted == YesNo.NO);
                    if (checkOrder)
                    {
                        _investHistoryUpdateEFRepository.ThrowException(ErrorCode.InvestProjectTradingProviderCanNotDelete);
                    }
                    item.Deleted = YesNo.YES;
                }

                foreach (var item in input.ProjectTradingProvider)
                {
                    // Thêm mới phân phối cho đại lý
                    if (item.Id == 0)
                    {
                        _dbContext.InvestProjectTradingProviders.Add(new ProjectTradingProvider
                        {
                            Id = (int)_investProjectTradingProviderEFRepository.NextKey(),
                            PartnerId = partnerId,
                            ProjectId = input.Id,
                            TradingProviderId = item.TradingProviderId,
                            CreatedBy = username,
                            CreatedDate = DateTime.Now,
                            Deleted = YesNo.NO,
                            TotalInvestmentSub = (input.HasTotalInvestmentSub == YesNo.YES) ? item.TotalInvestmentSub  : null
                        });
                    }
                    // Trường hợp Update phân phối đại lý có chứa hạn mức
                    else if (input.HasTotalInvestmentSub == YesNo.YES && item.TotalInvestmentSub != null)
                    {
                        var projectTradingProvider = _dbContext.InvestProjectTradingProviders.FirstOrDefault(x => x.Id == item.Id);
                        if (projectTradingProvider != null && item.TotalInvestmentSub != projectTradingProvider.TotalInvestmentSub)
                        {
                            projectTradingProvider.TotalInvestmentSub = item.TotalInvestmentSub;
                            projectTradingProvider.ModifiedBy = username;
                            projectTradingProvider.ModifiedDate = DateTime.Now;
                        };
                    }
                }
            }
            _dbContext.SaveChanges();

            using (TransactionScope scope = new()) {
                var listProjectTypeDb = _projectRepository.FindByProjectId(input.Id).ToList();

                //xóa những projectType không có trong List
                var removeList = listProjectTypeDb.Where(p => p.ProjectId == input.Id && !input.ProjectTypes.Contains(p.Type)).ToList();
                foreach (var removeItem in removeList)
                {
                    _projectRepository.DeleteProjectType(removeItem.Id);
                }

                foreach (var type in input.ProjectTypes) 
                {
                    if (type != InvestProjectTypes.NHA_RIENG && type != InvestProjectTypes.CAN_HO_CHUNG_CU && type != InvestProjectTypes.NHA_PHO_BIET_THU_DU_AN
                        && type != InvestProjectTypes.DAT_NEN_DU_AN && type != InvestProjectTypes.BIET_THU_NGHI_DUONG && type != InvestProjectTypes.CONDOTEL
                        && type != InvestProjectTypes.SHOPHOUSE && type != InvestProjectTypes.OFFICETEL)
                    {
                        throw new FaultException(new FaultReason($"Loại hình dự án không hợp lệ"), new FaultCode(((int)ErrorCode.InvestProjectTypeNotFound).ToString()), "");
                    }
                    var projectType = listProjectTypeDb.FirstOrDefault(p => p.Type == type && p.ProjectId == input.Id);
                    if (projectType == null) //không thấy thì thêm
                    {
                        _projectRepository.AddProjectType(new ProjectType
                        {
                            ProjectId = input.Id,
                            Type = type,
                        });
                    }
                }
                scope.Complete();
            }
            _projectRepository.CloseConnection();
        }
    }
}
