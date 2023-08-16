using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.Configs
{
    /// <summary>
    /// Cấu hình Msb
    /// </summary>
    public class MsbConfiguration
    {
        private string _baseUrl;
        public string BaseUrl
        {
            get => _baseUrl;
            set => _baseUrl = value?.Trim();
        }

        private string _mId;
        /// <summary>
        /// Mặc định của epic
        /// </summary>
        public string MId
        {
            get => _mId;
            set => _mId = value?.Trim();
        }

        private string _tId;
        /// <summary>
        /// Mặc định của epic
        /// </summary>
        public string TId
        {
            get => _tId;
            set => _tId = value?.Trim();
        }

        private string _accessCode;
        /// <summary>
        /// Mặc định của epic
        /// </summary>
        public string AccessCode
        {
            get => _accessCode;
            set => _accessCode = value?.Trim();
        }

        public int TimeOut { get; set; }
    }
}
