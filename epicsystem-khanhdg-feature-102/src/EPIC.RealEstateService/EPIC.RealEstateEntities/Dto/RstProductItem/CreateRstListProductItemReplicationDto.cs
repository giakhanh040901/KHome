using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class CreateRstListProductItemReplicationDto
    {
        public bool ChinhSachUuDaiCDT { get; set; }
        public bool TienIch { get; set; }
        public bool VatLieu { get; set; }
        public bool SoDoThietKe { get; set; }
        public bool HinhAnh { get; set; }
        public int ProductItemId { get; set; }
        public List<CreateRstProductItemReplicationDto> Items { get; set; }

    }
}
