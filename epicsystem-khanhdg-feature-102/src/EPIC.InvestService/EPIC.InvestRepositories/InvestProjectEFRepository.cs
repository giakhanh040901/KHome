using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.Project;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestProjectEFRepository : BaseEFRepository<Project>
    {
        public InvestProjectEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{Project.SEQ}")
        {
        }

        public PagingResult<ProjectDto> FindAll(FilterInvestProjectDto input, int? partnerId)
        {
            _logger.LogInformation($"{nameof(InvestProjectEFRepository)} -> {nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");
            PagingResult<ProjectDto> result = new();
            var query = from project in _epicSchemaDbContext.InvestProjects
                        where 
                        project.Deleted == YesNo.NO
                        && project.Status != null
                        && (input.Keyword == null || input.Keyword.Contains(project.InvCode) || input.Keyword.Contains(project.InvName))
                        && (input.Status == null || project.Status == input.Status)
                        && project.PartnerId == partnerId
                        select new ProjectDto
                        {
                            Id = project.Id,
                            PartnerId = project.PartnerId, 
                            OwnerId = project.OwnerId,   
                            GeneralContractorId = project.GeneralContractorId,      
                            InvCode = project.InvCode,
                            InvName = project.InvName,   
                            Content = project.Content,   
                            StartDate = project.StartDate,
                            EndDate = project.EndDate,
                            Image = project.Image,     
                            IsPaymentGuarantee = project.IsPaymentGuarantee,    
                            Area = project.Area,  
                            Longitude = project.Longitude,     
                            Latitude = project.Latitude,  
                            LocationDescription = project.LocationDescription,   
                            TotalInvestment = project.TotalInvestment,     
                            TotalInvestmentDisplay = project.TotalInvestmentDisplay,  
                            ProjectType = project.ProjectType,     
                            ProjectProgress = project.ProjectProgress,   
                            GuaranteeOrganization = project.GuaranteeOrganization,     
                            IsCheck = project.IsCheck,   
                            TradingProviderId = project.TradingProviderId,   
                            Status = project.Status,  
                            HasTotalInvestmentSub = project.HasTotalInvestmentSub, 
                        };
            result.TotalItems = query.Count();

            query = query.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }
    }
}
