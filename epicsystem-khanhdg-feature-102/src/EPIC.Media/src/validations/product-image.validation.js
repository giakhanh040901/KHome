const Joi = require('joi');
const { objectId } = require('./custom.validation');

const createProductImage = {
  body: Joi.object().keys({
    title: Joi.string().required(),
    type: Joi.string(),
    mainImg: Joi.string(),
    isFeatured: Joi.boolean(),
    status: Joi.string(),
    productId: Joi.string(),
    displayText: Joi.string().allow(''),
    content: Joi.string(),
    sort: Joi.number(),
    contentType: Joi.string(),
  }),
};

const getProductImage = {
  query: Joi.object().keys({
    title: Joi.string(),
    sortBy: Joi.string(),
    q: Joi.string(),
    type: Joi.string(),
    status: Joi.string(),
    productId: Joi.string(),
    limit: Joi.number().integer(),
    page: Joi.number().integer(),
  }).unknown(true)
};

const getProductImageDetail = {
  params: Joi.object().keys({
    imageId: Joi.string().custom(objectId),
  }),
};

const updateProductImage = {
  params: Joi.object().keys({
    productImageId: Joi.required().custom(objectId),
  }),
  body: Joi.object()
    .keys({
      title: Joi.string().required(),
      type: Joi.string(),
      mainImg: Joi.string(),
      isFeatured: Joi.boolean(),
      status: Joi.string(),
      productId: Joi.string(),
      displayText: Joi.string().allow(''),
      isFeatured: Joi.boolean(),
      trading_provider_id: Number,
      sort: Joi.number(),
      contentType: Joi.string(),
      content: Joi.string()
    })
    .min(1).unknown(true)
};

const deleteProductImage = {
  params: Joi.object().keys({
    productImageId: Joi.string().custom(objectId),
  }),
};

module.exports = {
  createProductImage,
  getProductImage,
  getProductImageDetail,
  updateProductImage,
  deleteProductImage,
};
