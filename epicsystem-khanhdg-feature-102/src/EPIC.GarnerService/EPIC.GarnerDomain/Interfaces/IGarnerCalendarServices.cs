using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerCalendarServices
    {
        List<GarnerCalendar> FindAll(int? workingYear);
        GarnerCalendar FindByWorkingDate(DateTime date);
        int AddByYear(CreateCalendarDto input);
        void Update(UpdateCalendarDto input);
    }
}
