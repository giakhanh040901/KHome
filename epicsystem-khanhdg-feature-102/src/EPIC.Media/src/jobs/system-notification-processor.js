const Queue = require('bull');
const config = require('../config/config');
const { emailService, notificationService, smsService, notificationConfig } = require('../services');
var logger = require('../helpers/logger');
var { pushAppNotification } = require('../helpers/firebase');
const { SentNotification } = require('../models');
const mongoose = require('mongoose');
const notificationQueue = new Queue('notification-queue', config.redisUrl);

notificationQueue.process(async function (job) {
  console.log('>>> Process notification started!', job.data);
  let hasError = {
    email: {
      state: false,
      log: '',
    },
    sms: {
      state: false,
      log: '',
    },
    app: {
      state: false,
      log: '',
    },
  };
  let inputData = job.data;
  let settings = [];

  if (inputData.trading_provider_id) {
    //Get notification config from Database by trading_provider_id
    console.log('trading_provider_id_____', inputData.trading_provider_id);
    let notificationConfiguration = await notificationConfig.getNotificationConfigByTradingProviderId(
      inputData.trading_provider_id
    );
    if (inputData.key === 'TK_THONGBAO_OTP') {
      notificationConfiguration = await notificationConfig.getNotificationConfigByTradingProviderId(999999);
    }
    let systemNotificationTemplates = await notificationService.getAllSystemNotificationByPartnerId(
      inputData.trading_provider_id
    );

    // let systemNotificationTemplates = await notificationService.getNewSystemNotificationByPartnerId(inputData.trading_provider_id);
    console.log('LENGTH', systemNotificationTemplates.length);
    if (systemNotificationTemplates.length == 0 && inputData.trading_provider_id != 0) {
      systemNotificationTemplates = await notificationService.getAllSystemNotificationByPartnerId(0);
    }
    if (systemNotificationTemplates) {
      systemNotificationTemplates.forEach((notificationByType) => {
        settings = settings.concat(notificationByType.settings);
      });
    }
    // }
    let notificationTemplate = settings.find((template) => {
      return template.key == inputData.key;
    });
    if (notificationTemplate) {
      if (notificationTemplate.isAdmin) {
        inputData.receiver.phone = notificationTemplate.adminPhone;
        inputData.receiver.email.address = notificationTemplate.adminEmail;
      }
      if (notificationTemplate.actions.includes('SEND_EMAIL')) {
        var emailContent = notificationTemplate.emailContent;
        try {
          var res = '';
          if (notificationTemplate.isAdmin) {
            let batchSend = notificationTemplate.adminEmail.map((emailAddress) => {
              return emailService.sendEmail(
                notificationConfiguration,
                emailAddress,
                notificationTemplate.titleAppContent,
                emailContent,
                inputData.data,
                notificationTemplate.emailContentType,
                inputData.attachments
              );
            });
            res = await Promise.all(batchSend);
          } else {
            res = await emailService.sendEmail(
              notificationConfiguration,
              inputData.receiver.email.address,
              notificationTemplate.titleAppContent,
              emailContent,
              inputData.data,
              notificationTemplate.emailContentType,
              inputData.attachments
            );
          }
          console.log('Email sent. ', inputData, res);
        } catch (err) {
          hasError.email.state = true;
          hasError.email.log = err;
          logger.error('>>> send email error', err);
        }
      }
      if (notificationTemplate.actions.includes('SEND_SMS')) {
        var smsContent = notificationTemplate.smsContent;
        if (inputData.data && inputData.receiver.phone) {
          Object.keys(inputData.data).forEach((keyToReplace) => {
            smsContent = smsContent.replace('['.concat(keyToReplace).concat(']'), inputData.data[keyToReplace]);
          });
        }
        try {
          let res = await smsService.sendSMS(notificationConfiguration, inputData.receiver.phone, smsContent);
          if (res.data.code != 0) {
            logger.error('>>> send sms error', res.data.message);
          }
        } catch (err) {
          hasError.sms.state = true;
          hasError.sms.log = err;
          logger.error('>>> call sms api error', err);
        }
      }

      if (notificationTemplate.actions.includes('PUSH_NOTIFICATION')) {
        var pushAppContent = notificationTemplate.pushAppContent;
        var titleAppContent = notificationTemplate.titleAppContent;
        if (inputData.data && inputData.receiver.fcm_tokens) {
          let title, body, refId;
          if (inputData.key !== 'CHAT_RECEIVE_MSG' && inputData.key !== 'CHAT_RECEIVE_DIRECT_MSG') {
            Object.keys(inputData.data).forEach((keyToReplace) => {
              pushAppContent = pushAppContent.replace('['.concat(keyToReplace).concat(']'), inputData.data[keyToReplace]);
              titleAppContent = titleAppContent.replace('['.concat(keyToReplace).concat(']'), inputData.data[keyToReplace]);
            });
            var savedNotification = await SentNotification.create({
              userId: inputData.receiver.userId,
              type: getTypeByKey(inputData.key),
              notification: {
                pushAppContentType: notificationTemplate.pushAppContentType,
                notificationTemplateName: titleAppContent,
                description: titleAppContent,
                pushAppContent: pushAppContent,
                actions: notificationTemplate.actions,
              },
              detail: pushAppContent,
              title: notificationConfiguration.pushAppTitle,
            });
            refId = savedNotification._id;
            title = notificationConfiguration.pushAppTitle;
            body = titleAppContent;
          } else {
            title = inputData.data?.sender?.fullName || 'Hỗ trợ khách hàng';
            body = inputData?.data?.Content || inputData.data?.payload?.text || 'Đã gửi 1 file đính kèm';
          }
          try {
            let result = await pushAppNotification(inputData.receiver.fcm_tokens, title, body, refId);
          } catch (err) {
            hasError.app.state = true;
            hasError.app.log = err;
            logger.error('>>> push app notification error', err);
          }
        }
      }
      if (hasError.app.state || hasError.sms.state || hasError.email.state) {
        throw new Error(JSON.stringify(hasError));
      }
    }
  }
});

function getTypeByKey(key) {
  if (key.includes('TK_')) {
    return 'HE_THONG';
  } else if (key.includes('TK_') || key.includes('DAU_TU_') || key.includes('INVEST_')) {
    return 'GIAO_DICH';
  }
  return 'HE_THONG';
}

module.exports = notificationQueue;
