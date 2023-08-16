using EPIC.Entities.Dto.PartnerCalendar;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerPartnerCalendarServices
    {
        List<GarnerPartnerCalendar> FindAll(int? workingYear);
        GarnerPartnerCalendar FindByWorkingDate(DateTime date);
        int AddByYear(CreatePartnerCalendarDto input);
        void Update(UpdateCalendarDto input);
    }
}
