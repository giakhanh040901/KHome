using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface ICalendarServices
    {
        List<Calendar> FindAll(int workingYear);
        Calendar FindByWorkingDate(DateTime date);
        int AddByYear(CreateCalendarDto input);
        int Update(UpdateCalendarDto input);
    }
}
