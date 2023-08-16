using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestApprove
{
    public class InvestCancelDto
    {
        /// <summary>
        /// ReferId
        /// </summary>
        public int Id { get; set; }
        public string CancelNote { get; set; }
    }
}
