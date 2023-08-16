namespace EPIC.Shared.RabbitMQ.Sms.Dto
{
    public class SendSmsDto
    {
        public string PhoneNumber { get; set; }
        public string Body { get; set; }
    }
}
