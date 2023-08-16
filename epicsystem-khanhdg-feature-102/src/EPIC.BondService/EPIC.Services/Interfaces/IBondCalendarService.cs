using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondCalendarService
    {
        List<BondCalendar> FindAll(int workingYear);
        BondCalendar FindByWorkingDate(DateTime date);
        int AddByYear(CreateCalendarDto input);
        int Update(UpdateCalendarDto input);
    }
}
