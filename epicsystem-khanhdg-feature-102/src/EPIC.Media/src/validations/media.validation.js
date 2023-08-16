const Joi = require('joi');
const { objectId } = require('./custom.validation');

const createMedia = {
  body: Joi.object().keys({
    title: Joi.string().required(),
    type: Joi.string(),
    mainImg: Joi.string(),
    isFeatured: Joi.boolean(),
    status: Joi.string(),
    productKey: Joi.string().allow(''),
    position: Joi.string().required(),
    images: Joi.array(),
    displayText: Joi.string().allow(''),
    content: Joi.string(),
    sort: Joi.number(),
    contentType: Joi.string(),
  }).unknown(true),
};

const getMedia = {
  query: Joi.object().keys({
    title: Joi.string(),
    sortBy: Joi.string(),
    q: Joi.string(),
    type: Joi.string(),
    status: Joi.string(),
    position: Joi.string(),
    productKey: Joi.string().allow(''),
    limit: Joi.number().integer(),
    page: Joi.number().integer(),
  }).unknown(true)
};

const getMediaDetail = {
  params: Joi.object().keys({
    mediaId: Joi.string().custom(objectId),
  }),
};

const updateMedia = {
  params: Joi.object().keys({
    mediaId: Joi.required().custom(objectId),
  }),
  body: Joi.object()
    .keys({
      title: Joi.string().required(),
      type: Joi.string(),
      mainImg: Joi.string(),
      isFeatured: Joi.boolean(),
      status: Joi.string(),
      productKey: Joi.string().allow(''),
      images: Joi.array(),
      position: Joi.required(),
      displayText: Joi.string().allow(''),
      trading_provider_id: Number,
      sort: Joi.number(),
      contentType: Joi.string(),
      content: Joi.string()
    })
    .min(1).unknown(true)
};

const deleteMedia = {
  params: Joi.object().keys({
    mediaId: Joi.string().custom(objectId),
  }),
};

module.exports = {
  createMedia,
  getMedia,
  getMediaDetail,
  updateMedia,
  deleteMedia,
};
