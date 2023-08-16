using EPIC.BondEntities.DataEntities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.PartnerCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondPartnerCalendarService
    {
        List<BondPartnerCalendar> FindAll(int workingYear);
        BondPartnerCalendar FindByWorkingDate(DateTime date);
        int AddByYear(CreatePartnerCalendarDto input);
        int Update(UpdatePartnerCalendarDto input);
    }
}
