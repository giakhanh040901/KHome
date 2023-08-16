using System;
using System.Collections.Generic;
using System.Text;

namespace EPIC.SIGN.CORE
{
    public class ApiResp
    {
        public string description {get;set;}
        public string error {get;set;}
        public int status {get;set;}
        public object obj { get; set; }
    }
}
