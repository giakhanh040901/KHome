const httpStatus = require('http-status');
const pick = require('../utils/pick');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
const { notificationService, notificationTemplateService, notificationConfig } = require('../services');
const Chance = require('chance');
const { notificationStatus, notificationType, contentType, notificationActionType, sentStatus } = require("../config/newsEnums");
const { systemNotificationTemplate, bondNotificationTemplate, investNotificationTemplate, garnerNotificationTemplate,
  realEstateNotificationTemplate,loyaltyNotificationTemplate, eventNotificationTemplate
} = require("../config/system-notification-template");
const chance = new Chance();
const config = require("../config/config");
const systemNotificationQueue = require('../jobs/system-notification-processor')
const pushNotificationQueue = require('../jobs/push-notification-processor')
const {func} = require("joi");
const {SentNotification, SendingList, Notification} = require("../models");
const _ = require('lodash');

const sendNotification = catchAsync(async (req, res) => {
  const notification = await notificationService.getNotificationById(req.body.notificationId);
  if(!notification) {
    return res.status(httpStatus.NOT_FOUND).send({'message': "Notification not found!"});
  }

  if(!notification.dateSent) {
    const filter = { _id: notification._id };
    const update = { $set: { dateSent: new Date()} };
    const options = {};

    Notification.updateOne(filter, update, options, (err, result) => {
      if (err) throw err;
    });
  }
  req.body.receivers.map( async (receiver) => {
    pushNotificationQueue.add({
      notification,
      receiver
    });
  })
  res.status(httpStatus.CREATED).send(notification);
});

const createNotification = catchAsync(async (req, res) => {
  req.body.trading_provider_id = req.user.trading_provider_id;
  req.body.creatorName = req.user.display_name;
  const notification = await notificationService.createNotification(req.body);
  res.status(httpStatus.CREATED).send(notification);
});

const sendSystemNotification = catchAsync(async (req, res) => {
  console.log("_________req.params",req.params);
  console.log("__________req.body",req.body);
  systemNotificationQueue.add({
    trading_provider_id: req.params.trading_provider_id,
    key: req.body.key,
    data: req.body.data,
    receiver: req.body.receiver,
    attachments: req.body.attachments
  })
  res.status(httpStatus.OK).send({ status: 'ok' });
});

const getNotificationList = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['title', 'q','statusSent','typeModule']);

  if(req.tradingProvidersCanAccess) {
    filter.trading_provider_id = req.tradingProvidersCanAccess
  }

  if(req.user.isTrader) {
    filter.status = notificationStatus.ACTIVE
  }
  if(!filter.status) {
    filter.status = { $ne: notificationStatus.DELETED }
  }
  if(filter.q) {
    filter.$or = [ {
      content: { "$regex": filter.q, "$options": "i" }
    }, {
      title: { "$regex": filter.q, "$options": "i" }
    },{
      code: { "$regex": filter.q, "$options": "i" }
    }]
    delete filter.q;
  } else {
    delete filter.q;
  }

  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "_id:desc";
  }
  options.select = ['_id', 'title', 'description', 'type', 'isFeatured', 'status', 'creatorName', 'user_type', 'createdAt','notificationTemplateId','dateSent','actions']

  let query = {};
  if (filter.typeModule === 'eLoyalty') {
    query = { ...filter, typeModule: 'eLoyalty' };
  } else {
    query = { ...filter, typeModule: { $exists: false } };
  }
  var result = await notificationService.queryNotifications(query, options);
  const promises = result.results.map(item => notificationService.getSendingListByNotification({ notification: item.id }, { limit: 999999, page: 1 }));

  const sendingLists = await Promise.all(promises);

  result.results.forEach( (item, index) => {
    const sendingList = sendingLists[index];

    //tong so thong bao gui
    item.totalSending = sendingList?.totalResults ?? 0;

    //so thong bao da gui (thanh cong, loi, cho xu ly)
    item.sentAPP = sendingList.results.filter(item => ['SENDING', 'SENT', 'SEND_ERROR'].includes(item.pushAppStatus)).length;

    item.sentSMS = sendingList.results.filter(item => ['SENDING', 'SENT', 'SEND_ERROR'].includes(item.sendSMSStatus)).length;

    item.sentEmail = sendingList.results.filter(item => ['SENDING', 'SENT', 'SEND_ERROR'].includes(item.sendEmailStatus)).length;
    if(item.sentAPP !== 0 || item.sentSMS !== 0 || item.sentEmail !== 0) {
      item.statusSent = sentStatus.SENT
    } else {
      item.statusSent = sentStatus.DRAFT
    }

    const filter = { _id: item._id };
    const update = { $set: { statusSent: item.statusSent, sentAPP: item.sentAPP , sentSMS: item.sentSMS, sentEmail: item.sentEmail} };
    const options = {};

    Notification.updateOne(filter, update, options, (err, result) => {
      if (err) throw err;
    });
  });

  res.send(result);
});

const getNotificationDetail = catchAsync(async (req, res) => {
  const notification = await notificationService.getNotificationById(req.params.notificationId);
  if (!notification || (!req.user.isSuperAdmin && notification.trading_provider_id != req.user.trading_provider_id)) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Notification not found');
  }
  res.send(notification);
});

const updateNotification = catchAsync(async (req, res) => {
  if(!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  const notification = await notificationService.updateNotificationById(req.params.notificationId, req.body);
  res.send(notification);
});

const deleteNotification = catchAsync(async (req, res) => {
  await notificationService.deleteNotificationById(req.params.notificationId);
  res.status(httpStatus.NO_CONTENT).send();
});

const updatePersonList = catchAsync(async (req, res) => {
  const exitsPersonsInList = await notificationService.getSendingListByNotificationId(req.params.notificationId);
  let personList = _.concat(exitsPersonsInList, req.body.sendingList);
  let uniqueList = _.uniqBy(personList, 'personCode');
  if(!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  const sendingList = await notificationService.updateSendingList(req.params.notificationId, uniqueList, req.user.trading_provider_id);
  res.send(sendingList);
});

//
const deletePersonInList = catchAsync(async (req, res) => {
  if(req.query.ids) {
    await notificationService.deleteManyPersonIds(req.query.ids.split(","))
  }
  res.status(httpStatus.NO_CONTENT).send();
})

const getSendingList = catchAsync(async (req, res) => {
  if (!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }

  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "createdAt:desc";
  }
  const statuses = pick(req.query, ['sendSMSStatus', 'pushAppStatus', 'sendEmailStatus','phoneNumber','fullName','email']);
  const notification = await notificationService.getNotificationById(req.params.notificationId);
  if (!notification) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Notification not found');
  }
  let filters = { notification: notification.id }
  if (statuses.phoneNumber) {
    filters.phoneNumber = { $regex: new RegExp(statuses.phoneNumber, 'i') };
  }

  if (statuses.fullName) {
    const searchName = statuses.fullName.toLowerCase();
    filters.fullName = { $regex: new RegExp(searchName, 'i') };
  }

  if (statuses.email) {
    filters.email = { $regex: new RegExp(statuses.email, 'i') };
  }
  if (statuses.sendEmailStatus) {
    filters.sendEmailStatus = {
      $in: statuses.sendEmailStatus.split(",")
    }
  }
  if (statuses.sendSMSStatus) {
    filters.sendSMSStatus = {
      $in: statuses.sendSMSStatus.split(",")
    }
  }
  if (statuses.pushAppStatus) {
    filters.pushAppStatus = {
      $in: statuses.pushAppStatus.split(",")
    }
  }

  const sendingList = await notificationService.getSendingListByNotification(filters, options);
  res.send(sendingList);
});

const createOrUpdateNotificationTemplate = catchAsync(async (req, res) => {
  await notificationService.createOrUpdateSystemNotification(req.user.trading_provider_id, req.body, req.query.type);
  res.status(httpStatus.NO_CONTENT).send();
});

const getSystemNotification = catchAsync(async (req, res) => {
  var configKeys = [];

  if(req.query.type === 'default') {
    let bondDefaultTemplate = _.cloneDeep(bondNotificationTemplate, true).map(template => {
      template.notificationTemplateName = "[BOND] ".concat(template.notificationTemplateName);
      return template
    })
    let investDefaultTemplate = _.cloneDeep(investNotificationTemplate, true).map(template => {
      template.notificationTemplateName = "[INVEST] ".concat(template.notificationTemplateName);
      return template
    })
    let GarnerDefaultTemplate = _.cloneDeep(garnerNotificationTemplate, true).map(template => {
      template.notificationTemplateName = "[GARNER] ".concat(template.notificationTemplateName);
      return template
    })
    let RealEstateDefaultTemplate = _.cloneDeep(realEstateNotificationTemplate, true).map(template => {
      template.notificationTemplateName = "[HOMES] ".concat(template.notificationTemplateName);
      return template
    })

    let LoyaltyDefaultTemplate = _.cloneDeep(loyaltyNotificationTemplate, true).map(template => {
      template.notificationTemplateName = "[LOYALTY] ".concat(template.notificationTemplateName);
      return template
    })

    let EventDefaultTemplate = _.cloneDeep(eventNotificationTemplate, true).map(template => {
      template.notificationTemplateName = "[EVENT] ".concat(template.notificationTemplateName);
      return template
    })

    configKeys = systemNotificationTemplate
      .concat(investDefaultTemplate)
      .concat(bondDefaultTemplate)
      .concat(GarnerDefaultTemplate)
      .concat(RealEstateDefaultTemplate)
      .concat(LoyaltyDefaultTemplate)
      .concat(EventDefaultTemplate)
    req.user.trading_provider_id = 0;
  }
  const systemNotification = await notificationService.getSystemNotificationByPartnerId(req.user.trading_provider_id, req.query.type);
  if(req.query.type === 'bond') {
    configKeys = bondNotificationTemplate
  }
  if(req.query.type === 'system') {
    configKeys = systemNotificationTemplate
  }
  if(req.query.type === 'real_estate_invest') {
    configKeys = investNotificationTemplate
  }
  if(req.query.type === 'garner') {
    configKeys = garnerNotificationTemplate
  }
  if(req.query.type === 'real_estate') {
    configKeys = realEstateNotificationTemplate
  }
  if(req.query.type === 'loyalty') {
    configKeys = loyaltyNotificationTemplate
  }
  if(req.query.type === 'event') {
    configKeys = eventNotificationTemplate
  }
  res.send({
    configKeys,
    value: systemNotification || []
  });
});


const getSentNotificationMobileList = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['type','typeModule']);
  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "createdAt:desc";
  }
  filter.userId = req.user.sub;
  let query = {};
  if (filter.typeModule === 'eLoyalty') {
    query = { ...filter, typeModule: 'eLoyalty' };
  } else {
    query = { ...filter, typeModule: { $exists: false } };
  }
  var result = await notificationService.querySentUserNotification(query, options);
  let returnNotifications = result.results.map(notification => {
    return {
      notification: {
        notificationTemplateName: notification.notification.notificationTemplateName,
        description: notification.notification.description,
      },
      type: notification.type,
      createdAt: notification.createdAt,
      updatedAt: notification.updatedAt,
      id: notification.id,
      isRead: notification.isRead,
    }
  });

  result.results = returnNotifications;
  res.send({
    status: 1,
    data: result,
    code: 200,
    message: 'OK'
  });
});

const getReadMobile = catchAsync(async (req, res) => {
  const result = await notificationService.setNotificationReadById(req.params.sentNotificationId);
  res.send(result);
});

const deleteNotificationMobileList = catchAsync(async (req, res) => {
  const userId = req.user.sub;
  const deleteNotification = await notificationService.deleteAllNotificationbyUserId(userId);
  res.send(deleteNotification);
})

const deleteSingleNotificationMobile = catchAsync(async (req, res) => {
  const id = req.params.id;
  const deleteNotification = await notificationService.deleteAllNotificationbyId(id);
  res.send(deleteNotification);
})

const readAllNotificationMobileList = catchAsync(async (req, res) => {
  const userId = req.user.sub;
  const readAllNotification = await notificationService.readAllNotificationbyUserId(userId);
  res.send(readAllNotification);
})

module.exports = {
  createNotification,
  getNotificationDetail,
  getNotificationList,
  updateNotification,
  deleteNotification,
  updatePersonList,
  getSendingList,
  createOrUpdateNotificationTemplate,
  getSystemNotification,
  sendSystemNotification,
  sendNotification,
  deletePersonInList,
  getReadMobile,
  getSentNotificationMobileList,
  deleteNotificationMobileList,
  readAllNotificationMobileList,
  deleteSingleNotificationMobile
};
