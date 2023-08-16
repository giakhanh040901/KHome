const Joi = require('joi');
const { objectId } = require('./custom.validation');

const createNews = {
  body: Joi.object().keys({
    code: Joi.string(),
    title: Joi.string().required(),
    content: Joi.string(),
    displayText: Joi.string().allow(''),
    contentType: Joi.string(),
    mainImg: Joi.string(),
    order: Joi.number(),
    isFeatured: Joi.boolean(),
    status: Joi.string(),
    type: Joi.string(),
  }).unknown(true),
};

const getNews = {
  query: Joi.object().keys({
    title: Joi.string(),
    sortBy: Joi.string(),
    q: Joi.string(),
    limit: Joi.number().integer(),
    page: Joi.number().integer(),
  }).unknown(true)
};

const getNewsDetail = {
  params: Joi.object().keys({
    newsId: Joi.string().custom(objectId),
  }),
};

const updateNews = {
  params: Joi.object().keys({
    newsId: Joi.required().custom(objectId),
  }),
  body: Joi.object()
    .keys({
      title: Joi.string().required(),
      content: Joi.string(),
      mainImg: Joi.string(),
      contentType: Joi.string(),
      order: Joi.number(),
      isFeatured: Joi.boolean(),
      status: Joi.string(),
      type: Joi.string()
    })
    .min(1)
    .unknown(true)
};

const deleteNews = {
  params: Joi.object().keys({
    newsId: Joi.string().custom(objectId),
  }),
};

module.exports = {
  createNews,
  getNews,
  getNewsDetail,
  updateNews,
  deleteNews,
};
