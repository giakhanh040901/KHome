using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.PartnerCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface IPartnerCalendarServices
    {
        List<PartnerCalendar> FindAll(int workingYear);
        PartnerCalendar FindByWorkingDate(DateTime date);
        int AddByYear(CreatePartnerCalendarDto input);
        int Update(UpdatePartnerCalendarDto input);
    }
}
