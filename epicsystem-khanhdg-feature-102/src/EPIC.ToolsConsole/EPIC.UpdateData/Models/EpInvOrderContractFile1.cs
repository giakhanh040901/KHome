﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvOrderContractFile1
    {
        [Key]
        public decimal Id { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? ContractTempId { get; set; }
        public string FileTempUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string FileSignatureUrl { get; set; }
        public string FileScanUrl { get; set; }
        public string IsSign { get; set; }
        public decimal? PageSign { get; set; }
        public string FileTempPdfUrl { get; set; }
    }
}
