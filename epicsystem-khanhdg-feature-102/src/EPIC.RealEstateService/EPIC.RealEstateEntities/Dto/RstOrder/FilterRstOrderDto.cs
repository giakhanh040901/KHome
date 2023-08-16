using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class FilterRstOrderDto : PagingRequestBaseDto
    {
        private string _name;
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _code;
        [FromQuery(Name = "code")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Nguồn đặt lệnh: 1: ONLINE, 2: OFFLINE
        /// </summary>
        [FromQuery(Name = "source")]
        public int? Source { get; set; }
        
        [FromQuery(Name = "saleOrderId")]
        public int? SaleOrderId { get; set; }

        private string _phone;
        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _idNo;
        [FromQuery(Name = "idNo")]
        public string IdNo
        {
            get => _idNo;
            set => _idNo = value?.Trim();
        }

        private string _taxCode;
        [FromQuery(Name = "taxCode")]
        public string TaxCode
        {
            get => _taxCode;
            set => _taxCode = value?.Trim();
        }

        private string _contractCode;
        [FromQuery(Name = "contractCode")]
        public string ContractCode
        {
            get => _contractCode;
            set => _contractCode = value?.Trim();
        }

        private string _cifCode;
        [FromQuery(Name = "cifCode")]
        public string CifCode
        {
            get => _cifCode;
            set => _cifCode = value?.Trim();
        }
        /// <summary>
        /// Nguồn đặt lệnh trả về FE (1: Quản trị viên, 2: Khách hàng, 3: Sale)
        /// </summary>
        [FromQuery(Name = "orderer")]
        public int? Orderer { get; set; }

        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }
        
        /// <summary>
        /// Ngày hợp đồng cọc
        /// </summary>
        [FromQuery(Name = "depositDate")]
        public DateTime? DepositDate { get; set; }

        private string _contractCodeGen;
        [FromQuery(Name = "contractCodeGen")]
        public string ContractCodeGen
        {
            get => _contractCodeGen;
            set => _contractCodeGen = value?.Trim();
        }

        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }
    }
}
