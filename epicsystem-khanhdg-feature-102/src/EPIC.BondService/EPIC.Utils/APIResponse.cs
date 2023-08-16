using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils
{
    public class APIResponse
    {
        public StatusCode Status { get; set; }
        public object Data { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

        public APIResponse(Utils.StatusCode status, object data, int code, string message)
        {
            Status = status;
            Data = data;
            Code = code;
            Message = message;
        }

        public APIResponse(object data)
        {
            Status = StatusCode.Success;
            Data = data;
            Code = 200;
            Message = "Ok";
        }

        public APIResponse()
        {
            Status = StatusCode.Success;
            Data = null;
            Code = 200;
            Message = "Ok";
        }
    }

    public class APIResponse<T> : APIResponse
    {
        public new T Data { get; set; }

        public APIResponse(Utils.StatusCode status, T data, int code, string message) : base(status, data, code, message)
        {
            Status = status;
            Data = data;
            Code = code;
            Message = message;
        }

        public APIResponse(T data) : base(data)
        {
            Data = data;
        }
    }

    public enum StatusCode
    {
        Success = 1,
        Error = 0
    }
}
