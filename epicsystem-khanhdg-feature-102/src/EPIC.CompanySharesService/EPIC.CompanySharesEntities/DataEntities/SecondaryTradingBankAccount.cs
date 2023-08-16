using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class SecondaryTradingBankAccount
    {
        public int Id { get; set; }
        public int SecondaryId { get; set; }
        public int BusinessCustomerBankAccId { get; set; }
        public string Status { get; set; }

        #region audit
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        #endregion
    }
}
