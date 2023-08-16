using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConfigModel
{
    /// <summary>
    /// Model config http api, để gọi chéo nhau,
    /// api nào không dùng có thể bỏ qua
    /// </summary>
    public class SharedApiConfiguration
    {
        public string ApiMedia { get; set; }
        public string ApiQrCode { get; set; }
        public string ApiDocxToPdf { get; set; }
        public string ApiSignServer { get; set; }
        public string ApiBond { get; set; }
        public string ApiCore { get; set; }
        public string ApiNotification { get; set; }
    }
}
