const cron = require('cron');
const { Notification} = require('../models');
const { notificationService } = require('../services');
const notificationController = require('../controllers/notification.controller')
const pushNotificationQueue = require('../jobs/push-notification-processor')
const { sentStatus } = require('../config/newsEnums');

// stg delay 30s, nhan 1 thong bao
// prd delay 300s, nhan 2 thong bao
const job = new cron.CronJob('0 * * * * *', async () => {
  try {
    const notifications = await Notification.find({
      appointment: { $exists: true, $lt: new Date(Date.now() - 120 * 1000)}
    }).lean();

    await Promise.all(
      notifications.map(async (notification) => {
        let options = {
          limit: '999999',
          page: '1',
        }
        const filters = {
          notification: notification._id,
          $or: [
            { pushAppStatus: { $in: ['DRAFT'] } },
            { sendEmailStatus: { $in: ['DRAFT'] } },
            { sendSMSStatus: { $in: ['DRAFT'] } }
          ],
          $or: [
            { isLock: { $ne: true } },
            { isLock: { $exists: false } }
          ]
        };
        let sendingList = await notificationService.getSendingListByNotification(filters, options);
        if(notification.appointment < (new Date(Date.now() - 60 * 1000))) {

          sendingList.results.forEach((receiver) => {
            console.log('receiver: ', receiver);
            pushNotificationQueue.add({
              notification,
              receiver
            });
            notificationService.updateIsLock(receiver._id, true)
          });
          const notificationDoc = await Notification.findById(notification._id).lean();

          if (!notificationDoc.dateSent) {
            await Notification.updateOne(
              { _id: notificationDoc._id },
              { $set: { dateSent: new Date() } }
            );
          }
        }

      })
    );
  } catch (error) {
    console.log('Lỗi khi lấy dữ liệu từ MongoDB: ' + error);
  }
});

module.exports = job;
