const Joi = require('joi');
const { objectId } = require('./custom.validation');

const createFeedback = {
  body: Joi.object().keys({
    title: Joi.string().required(),
    detail: Joi.string(),
    attachments: Joi.array().items(Joi.object({
      name: Joi.string().allow(''),
      url: Joi.string().allow('')
    })),
    category: Joi.string(),
    reply: Joi.array().items(Joi.object({
      senderName: Joi.string().allow(''),
      senderId: Joi.number()
    })),
  }),
};

const getFeedback = {
  query: Joi.object().keys({
    title: Joi.string(),
    sortBy: Joi.string(),
    q: Joi.string(),
    limit: Joi.number().integer(),
    page: Joi.number().integer(),
  }).unknown(true)
};

const getFeedbackDetail = {
  params: Joi.object().keys({
    feedbackId: Joi.string().custom(objectId),
  }),
};

const updateFeedback = {
  params: Joi.object().keys({
    feedbackId: Joi.required().custom(objectId),
  }),
  body: Joi.object().keys({
    title: Joi.string().required(),
    detail: Joi.string(),
    attachments: Joi.array().items(Joi.object({
      name: Joi.string().allow(''),
      url: Joi.string().allow('')
    })),
    category: Joi.string(),
    reply: Joi.array().items(Joi.object({
      senderName: Joi.string().allow(''),
      senderId: Joi.number()
    })),
  }).min(1).unknown(true)
};

const deleteFeedback = {
  params: Joi.object().keys({
    feedbackId: Joi.string().custom(objectId),
  }),
};

module.exports = {
  createFeedback,
  getFeedback,
  getFeedbackDetail,
  updateFeedback,
  deleteFeedback,
};
