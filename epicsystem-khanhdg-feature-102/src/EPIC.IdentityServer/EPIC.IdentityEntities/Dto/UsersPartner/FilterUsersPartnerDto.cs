using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.UsersPartner
{
    public class FilterUsersManagerDto : PagingRequestBaseDto
    {
        private string _status;
        /// <summary>
        /// Lọc theo trạng thái: A: ACTIVE, D: DEACTIVE, X: IS_DELETED(đã xóa)
        /// </summary>
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }
    }
}
