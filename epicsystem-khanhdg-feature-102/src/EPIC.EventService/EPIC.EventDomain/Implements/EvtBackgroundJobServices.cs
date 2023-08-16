using EPIC.DataAccess.Base;
using EPIC.Notification.Services;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.Filter;
using Hangfire;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Implements
{
    public class EvtBackgroundJobServices
    {
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly EpicSchemaDbContext _dbContext;
        public EvtBackgroundJobServices(
            EventNotificationServices eventNotificationServices,
            EpicSchemaDbContext dbContext)
        {
            _eventNotificationServices = eventNotificationServices;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gửi thông báo sự kiện sắp diễn ra
        /// </summary>
        [Queue(HangfireQueues.Event)]
        [HangfireLogEverything]
        public async Task SendEventUpComing(int eventDetailId)
        {
            await _eventNotificationServices.SendEventUpComing(eventDetailId);
        }

        /// <summary>
        /// Gửi thông báo sự kiện kết thúc
        /// </summary>
        [Queue(HangfireQueues.Event)]
        [HangfireLogEverything]
        public async Task SendEventFinished(int eventId)
        {
            await _eventNotificationServices.SendAdminNotifyEventFinished(eventId);
        }
    }
}
