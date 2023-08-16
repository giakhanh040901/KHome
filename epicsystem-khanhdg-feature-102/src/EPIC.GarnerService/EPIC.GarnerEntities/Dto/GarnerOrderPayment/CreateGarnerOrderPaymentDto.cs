using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrderPayment
{
    public class CreateGarnerOrderPaymentDto
    {
        public long OrderId { get; set; }
        public int TradingBankAccId { get; set; }
        public DateTime? TranDate { get; set; }
        public int? TranType { get; set; }
        public int? TranClassify { get; set; }
        public int? PaymentType { get; set; }
        public decimal? PaymentAmount { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
    }
}
