const Queue = require('bull');
const config = require("../config/config");
const { emailService, notificationService, smsService, notificationConfig } = require('../services');
var logger = require('../helpers/logger');
const { SendingList, SentNotification } = require("../models");
const { pushAppNotification } = require("../helpers/firebase");


const pushNotificationQueue = new Queue('push-notification-queue', config.redisUrl);

pushNotificationQueue.process(async function (job) {
  console.log(">>> Process push notification started!", job.data)
  let inputData = job.data;
  console.log("_____INPUT",inputData);
  let notificationConfiguration = await notificationConfig.getNotificationConfigByTradingProviderId(inputData.notification.trading_provider_id)
  const receiver = await notificationService.getPersonInList(inputData.receiver.id);
  if(!receiver) {
    return;
  }

  if(inputData.notification.actions.includes('PUSH_NOTIFICATION')) {
    var pushAppContent = inputData.notification.appNotificationDesc;

    if(inputData.receiver.fcmTokens) {
      try {
        var savedNotification = await SentNotification.create({
          userId: inputData.receiver.personCode,
          type: inputData.notification.type,
          notification: {
            pushAppContentType: inputData.notification.appNotifContentType,
            notificationTemplateName: inputData.notification.title,
            description: inputData.notification.appNotificationDesc,
            pushAppContent: inputData.notification.notificationContent,
            actions: inputData.notification.actions
          },
          detail: inputData.notification.notificationContent,
          title: inputData.notification.title,
          notificationTemplateName: inputData.notification.title,

          isNavigation: inputData.notification.isNavigation,
          navigationType: inputData.notification.navigationType,
          levelOneNavigation: inputData.notification.levelOneNavigation,
          secondLevelNavigation: inputData.notification.secondLevelNavigation,
          navigationLink: inputData.notification.navigationLink,
        });

        let result = await pushAppNotification(inputData.receiver.fcmTokens, notificationConfiguration.pushAppTitle, inputData.notification.appNotificationDesc, savedNotification._id, inputData.notification.mainImg);
        if(result) {
          receiver.pushAppStatus = 'SENT';
        }else {
          receiver.pushAppStatus = 'SEND_ERROR';
          receiver.appError = JSON.stringify({error: "messaging/registration-token-not-registered"});
        }
      } catch (err) {
        receiver.pushAppStatus = 'SEND_ERROR';
        console.log('>>> send app error', err)
        logger.error('>>> push app notification error', err);

        receiver.appError = err;

      }
    }
  }
  const smsPromise = inputData.notification.actions.includes('SEND_SMS') ? smsService.sendSMS(notificationConfiguration, inputData.receiver.phoneNumber, inputData.notification.smsContent) : null;
  const emailPromise = inputData.notification.actions.includes('SEND_EMAIL') ? emailService.sendEmail(notificationConfiguration, inputData.receiver.email, inputData.notification.title, inputData.notification.emailContent, {}, inputData.notification.emailContentType, inputData.attachments) : null;

  try {
    if (smsPromise) {
      let smsRes = await smsPromise;
      receiver.sendSMSStatus = smsRes.data.code === 0 ? 'SENT' : 'SEND_ERROR';
      if (smsRes.data.code !== 0) {
        console.log('>>> send sms error', smsRes.data.message);
        logger.error('>>> send sms error', smsRes.data.message);
        receiver.smsError = smsRes.data.message ? smsRes.data : 'Lỗi hệ thống';
      }
    }

    if (emailPromise) {
      let emailRes = await emailPromise;
      receiver.sendEmailStatus = emailRes.status === 'success' ? 'SENT' : 'SEND_ERROR';
      if (emailRes.status !== 'success') {
        console.log('>>> send email error', emailRes.error);
        logger.error('>>> send email error', emailRes.error);
        receiver.emailError = emailRes.error;
      }
    }
  } catch (err) {
    console.log('>>> send sms/email error', err);
    logger.error('>>> send sms/email error', err);
    receiver.sendSMSStatus = 'SEND_ERROR';
    receiver.smsError = err;
    receiver.sendEmailStatus = 'SEND_ERROR';
    receiver.emailError = err;
  }

  // if(inputData.notification.actions.includes('SEND_SMS') ) {
  //   let notificationTemplate = inputData.notification;
  //   var smsContent = notificationTemplate.smsContent;
  //   try {
  //     let res = await smsService.sendSMS(notificationConfiguration, inputData.receiver.phoneNumber, smsContent)
  //     receiver.sendSMSStatus = 'SENT';
  //     if (res.data.code != 0) {
  //       console.log('>>> send sms error', res.data.message)
  //       logger.error('>>> send sms error', res.data.message);
  //       receiver.sendSMSStatus = 'SEND_ERROR';
  //       // receiver.smsError = res.data.message;
  //       if(res.data.message) {
  //         receiver.smsError = res.data;
  //       } else {
  //         receiver.smsError = `Lỗi hệ thống`;
  //       }
  //     }
  //   }catch (err) {
  //     console.log('>>> send sms error', err)
  //     logger.error('>>> call sms api error', err);
  //     receiver.sendSMSStatus = 'SEND_ERROR';
  //     receiver.smsError = err;
  //   }
  // }

  // if(inputData.notification.actions.includes('SEND_EMAIL')) {
  //   var emailContent = inputData.notification.emailContent;
  //   try {
  //     let res = await emailService.sendEmail(notificationConfiguration, inputData.receiver.email, inputData.notification.title, emailContent, {},inputData.notification.emailContentType, inputData.attachments)
  //     receiver.sendEmailStatus = 'SENT';
  //   } catch (err) {
  //     console.log('>>> send email error', err)
  //     logger.error('>>> send email error', err);
  //     receiver.sendEmailStatus = 'SEND_ERROR';
  //     receiver.emailError = err;
  //   }
  // }

  await receiver.save();
});

module.exports = pushNotificationQueue;
