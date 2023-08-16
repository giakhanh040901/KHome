using EPIC.Utils;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrderDetail
{
    public class CreateEvtOrderDetailDto
    {
        public int? Id { get; set; }
        /// ID loại vé
        /// </summary>
        public int TicketId { get; set; }
        /// ID loại vé
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
}
