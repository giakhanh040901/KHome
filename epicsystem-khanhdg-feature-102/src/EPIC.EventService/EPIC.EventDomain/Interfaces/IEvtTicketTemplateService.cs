using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtTicketTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtTicketTemplateService
    {
        /// <summary>
        /// Thêm mẫu vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ResponseEvtTicketTemplateDto Add(CreateEvtTicketTemplateDto input);

        /// <summary>
        /// sửa mẫu vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ResponseEvtTicketTemplateDto Update(UpdateEvtTicketTemplateDto input);

        /// <summary>
        /// danh sách mẫu vé của từng event
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<ResponseEvtTicketTemplateDto> FindAll(FilterEvtTicketTemplateDto input);

        /// <summary>
        /// kích hoạt và hủy mẫu vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void ChangeStatus(int id);
    }
}
