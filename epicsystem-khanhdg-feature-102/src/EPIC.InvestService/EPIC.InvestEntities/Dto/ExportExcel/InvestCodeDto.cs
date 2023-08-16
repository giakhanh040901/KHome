using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ExportExcel
{
    public class InvestCodeDto
    {
        public int? Id { get; set; }
        public int? Stt { get; set; }

        private string _invCode;
        public string InvCode
        {
            get => _invCode;
            set => _invCode = value?.Trim();
        }

        private string _ownerName;   
        public string OwnerName 
        { 
            get => _ownerName; 
            set => _ownerName = value?.Trim();
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public double TotalInvestment { get; set; }
        public double TotalAmountRemain { get; set; }
        public double SumTotalValueOrder { get; set; }
    }
}
