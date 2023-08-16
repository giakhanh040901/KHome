using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Shared.RabbitMQ
{
    public class RabbitMqQueueName
    {
        public string EmailQueue { get; set; }
        public string SmsQueue { get; set; }
    }

    public class RabbitMqConfiguration
    {
        public string Hostname { get; set; }
        public RabbitMqQueueName QueueNames { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
    }
}
