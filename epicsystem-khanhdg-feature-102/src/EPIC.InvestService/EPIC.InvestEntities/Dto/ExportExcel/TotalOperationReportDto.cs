using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ExportExcel
{
    public class TotalOperationReportDto
    {
        public int Stt { get; set; }
        public DateTime? Date { get; set; }
        public int? PendingContractNumber { get; set; }
        public int? ApprovedContractNumber { get; set; }
        public int? RequestDeliveryContractNumber { get; set; }
        public int? PrintedContractNumber { get; set; }
        public int? AssignedToSaleContractNumber { get; set; }
        public int? CompletedContractNumber { get; set; }
    }
}
