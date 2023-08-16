const Joi = require('joi');
const { objectId } = require('./custom.validation');
const mongoose = require("mongoose");

const createNotification = {
  body: Joi.object().keys({
    title: Joi.string().required(),
    type: Joi.string().required(),
    appNotifContentType: Joi.string().required(),
    notificationContent: Joi.string().allow(''),
    appNotificationDesc: Joi.string().required(),
    smsContent: Joi.string().allow(''),
    emailContent: Joi.string().allow(''),
    mainImg: Joi.string(),
    description: Joi.string(),
    actions: Joi.array().required(),
    isFeatured: Joi.boolean(),
    allowShare: Joi.boolean(),
    status: Joi.string(),
  }).unknown(true)
};

const sendNotification = {
  body: Joi.object().keys({
    receivers: Joi.array().required(),
    notificationId: Joi.string().required()
  }).unknown(true)
};

const getNotification = {
  query: Joi.object().keys({
    title: Joi.string(),
    sortBy: Joi.string(),
    limit: Joi.number().integer(),
    page: Joi.number().integer(),
  }).unknown(true)
};

const getNotificationDetail = {
  params: Joi.object().keys({
    notificationId: Joi.string().custom(objectId),
  }),
};

const updateNotification = {
  params: Joi.object().keys({
    notificationId: Joi.required().custom(objectId),
  }),
  body: Joi.object()
    .keys({
      title: Joi.string().required(),
      type: Joi.string().required(),
      appNotifContentType: Joi.string().required(),
      notificationContent: Joi.string().allow(''),
      appNotificationDesc: Joi.string().required(),
      smsContent: Joi.string().allow(''),
      description: Joi.string().allow(''),
      emailContent: Joi.string().allow(''),
      emailContentType: Joi.string().allow(''),
      mainImg: Joi.string(),
      actions: Joi.array().required(),
      isFeatured: Joi.boolean(),
      allowShare: Joi.boolean(),
      status: Joi.string()
    })
    .min(1)
    .unknown(true)
};

const deleteNotification = {
  params: Joi.object().keys({
    notificationId: Joi.string().custom(objectId),
  }),
};

const getSendingList = {
  params: Joi.object().keys({
    notificationId: Joi.string().custom(objectId),
  }),
};

const deleteSendingList = {
  query: Joi.object().keys({
    ids: Joi.string()
  }),
};

const setReadValidation = {
  params: Joi.object().keys({
    sentNotificationId: Joi.string().custom(objectId)
  }),
};

let personInList = Joi.object().keys({
  // fullName: Joi.string().allow(''),
  // personCode: Joi.string().allow(''),
  // phoneNumber: Joi.string().allow(''),
  // email: Joi.string().allow(''),
  // pushAppStatus: Joi.string().allow(''),
  // sendSMSStatus: Joi.string().allow(''),
  // sendEmailStatus: Joi.string().allow(''),
  // fcmTokens: Joi.array()
}).unknown(true)

const addPersonToSendingList = {
  params: Joi.object().keys({
    notificationId: Joi.string().custom(objectId),
  }),
  body: Joi.object()
    .keys({
      sendingList: Joi.array().items(personInList).required()
    })
    .min(1)
    .unknown(true)

}

let createSystemNotification = Joi.object().keys({
  params: Joi.object().keys({
    trading_provider_id: Joi.string()
  }),
  body: Joi.object().keys({
    key: Joi.string().required(),
    data: Joi.object().required(),
    receiver: Joi.object().keys({
      phone: Joi.string().allow(''),
      email: Joi.string().allow(''),
      userId: Joi.string().allow(''),
      fcm_tokens: Joi.array(),
    }).unknown(true),
    attachments: Joi.array()
  }).unknown(true),
  query: Joi.object().keys({
    type: Joi.string()
  })
})

const createOrUpdateSystemNotificationValidation = {
  body: Joi.array().items(
      Joi.object().keys({
        key: Joi.string(),
        notificationTemplateName: Joi.string(),
        // description: Joi.string().allow(''),
        actions: Joi.array().required(),
        emailContentType: Joi.string().allow(''),
        pushAppContentType: Joi.string().allow(''),
        smsContentType: Joi.string().allow(''),
        emailContent: Joi.string().allow(''),
        pushAppContent: Joi.string().allow(''),
        smsContent: Joi.string().allow(''),
    }).unknown(true)
  ).required(),
  query: Joi.object().keys({
    type: Joi.string()
  }),
};

const saveNotificationConfig = {
  body: Joi.object().keys({
    smsConfig: Joi.object().keys({
      USERNAME: Joi.string().allow(''),
      PASSWORD: Joi.string().allow(''),
      BRANDNAME: Joi.string().allow('')
    }).required(),
    stmpConfig: Joi.object().keys({
      SMTP_HOST: Joi.string().allow(''),
      SMTP_PORT: Joi.number(),
      SMTP_USERNAME: Joi.string().allow(''),
      SMTP_PASSWORD:  Joi.string().allow(''),
      EMAIL_FROM: Joi.string().allow(''),
      EMAIL_BRAND_NAME: Joi.string().allow(''),
    }).required(),
    pushAppTitle: Joi.string().allow('')
  }).unknown(true)
};

module.exports = {
  createNotification,
  getNotification,
  getNotificationDetail,
  updateNotification,
  deleteNotification,
  addPersonToSendingList,
  getSendingList,
  createOrUpdateSystemNotificationValidation,
  createSystemNotification,
  sendNotification,
  deleteSendingList,
  setReadValidation,
  saveNotificationConfig
};
