using EPIC.EventEntites.Dto.EvtAdminEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtAdminEventService 
    {
        /// <summary>
        /// Thêm admin
        /// </summary>
        /// <param name="input"></param>
        void Add(CreateEvtAdminEventDto input);
        /// <summary>
        /// Xóa admin
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// Dnah sách admin
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        IEnumerable<EvtAdminEventDto> FindAll(int eventId);
    }
}
