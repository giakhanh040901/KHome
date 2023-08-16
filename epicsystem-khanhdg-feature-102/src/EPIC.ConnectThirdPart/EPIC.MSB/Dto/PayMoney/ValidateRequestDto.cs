using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.PayMoney
{
    public class ValidateRequestDto
    {
        public long RequestId { get; set; }
        private string _tId;
        public string TId
        {
            get => _tId;
            set => _tId = value != null ? value.Trim() : throw new ArgumentException(nameof(TId));
        }

        private string _mId;
        public string MId
        {
            get => _mId;
            set => _mId = value != null ? value.Trim() : throw new ArgumentException(nameof(MId));
        }

        private string _accessCode;
        public string AccessCode
        {
            get => _accessCode;
            set => _accessCode = value != null ? value.Trim() : throw new ArgumentException(nameof(AccessCode));
        }
    }
}
