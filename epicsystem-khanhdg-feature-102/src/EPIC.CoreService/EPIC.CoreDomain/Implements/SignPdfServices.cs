using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.Utils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SignPdfDto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class SignPdfServices : ISignPdfServices
    {
        private readonly ILogger<SignPdfServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly SharedSignServerApiUtils _sharedSignServerApiUtils;

        public SignPdfServices(
            ILogger<SignPdfServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            SharedSignServerApiUtils sharedSignServerApiUtils,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _sharedSignServerApiUtils = sharedSignServerApiUtils;
            _mapper = mapper;
        }

        public ExportResultDto SignPdf(SignPdfDto dto)
        {
            //var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            result.fileDownloadName = dto.FileDownloadName;
            result.fileData = _sharedSignServerApiUtils.RequestSignBase64(dto);
            return result;
        }
    }
}
