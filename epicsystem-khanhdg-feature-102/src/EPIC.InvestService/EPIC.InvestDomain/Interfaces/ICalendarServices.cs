using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain
{
    public interface ICalendarServices 
    {
        List<Calendar> FindAll(int workingYear);
        Calendar FindByWorkingDate(DateTime date);
        int AddByYear(CreateCalendarDto input);
        int Update(UpdateCalendarDto input);
    }
}
