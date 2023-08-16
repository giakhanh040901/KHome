using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.TradingFirstMessage
{
    public class ViewTradingFirstMessageDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string Message { get; set; }
    }
}
