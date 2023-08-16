const Joi = require('joi');
const { objectId } = require('./custom.validation');

const createNotificationTemplate = {
  body: Joi.object().keys({
    title: Joi.string().required(),
    type: Joi.string().required(),
    contentType: Joi.string().allow(''),
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
    externalParams: Joi.string().allow(''),
    externalEvent: Joi.string().allow(''),
    actionView: Joi.string().allow('')
  }).unknown(true)
};

const getNotificationTemplate = {
  query: Joi.object().keys({
    title: Joi.string(),
    sortBy: Joi.string(),
    limit: Joi.number().integer(),
    page: Joi.number().integer(),
  }).unknown(true)
};

const getNotificationTemplateDetail = {
  params: Joi.object().keys({
    notificationId: Joi.string().custom(objectId),
  }),
};

const updateNotificationTemplate = {
  params: Joi.object().keys({
    notificationTemplateId: Joi.required().custom(objectId),
  }),
  body: Joi.object()
    .keys({
      title: Joi.string().required(),
      type: Joi.string().required(),
      contentType: Joi.string().allow(''),
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
      externalParams: Joi.string().allow(''),
      externalEvent: Joi.string().allow(''),
      actionView: Joi.string().allow('')
    })
    .min(1)
    .unknown(true)
};

const deleteNotificationTemplate = {
  params: Joi.object().keys({
    notificationTemplateId: Joi.string().custom(objectId),
  })
};

module.exports = {
  createNotificationTemplate,
  getNotificationTemplate,
  getNotificationTemplateDetail,
  updateNotificationTemplate,
  deleteNotificationTemplate,
};
