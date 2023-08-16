using EPIC.EntitiesBase.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.BusinessCustomer
{
    public class FilterBusinessCustomerDto : PagingRequestBaseDto
    {
        private string _isCheck;
        /// <summary>
        /// Lọc theo is check
        /// </summary>
        public string IsCheck 
        { 
            get => _isCheck; 
            set => _isCheck = value?.Trim(); 
        }
        private string _name;
        /// <summary>
        /// Lọc theo name
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _phone;
        /// <summary>
        /// Lọc theo phone
        /// </summary>
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _email;
        /// <summary>
        /// lọc theo email
        /// </summary>
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }
    }
}
