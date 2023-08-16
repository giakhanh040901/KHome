const Joi = require('joi');
const { objectId } = require('./custom.validation');

const createKnowledge = {
  body: Joi.object().keys({
    title: Joi.string().required(),
    content: Joi.string(),
    displayText: Joi.string().allow(''),
    contentType: Joi.string(),
    mainImg: Joi.string(),
    isFeatured: Joi.boolean(),
    status: Joi.string(),
    type: Joi.string(),
    category: Joi.string(),
    order: Joi.number(),
  }),
};

const getKnowledge = {
  query: Joi.object().keys({
    title: Joi.string(),
    sortBy: Joi.string(),
    q: Joi.string(),
    limit: Joi.number().integer(),
    page: Joi.number().integer(),
  }).unknown(true)
};

const getKnowledgeDetail = {
  params: Joi.object().keys({
    knowledgeId: Joi.string().custom(objectId),
  }),
};

const updateKnowledge = {
  params: Joi.object().keys({
    knowledgeId: Joi.required().custom(objectId),
  }),
  body: Joi.object()
    .keys({
      title: Joi.string().required(),
      content: Joi.string(),
      contentType: Joi.string(),
      mainImg: Joi.string(),
      isFeatured: Joi.boolean(),
      status: Joi.string(),
      type: Joi.string()
    })
    .min(1)
    .unknown(true)
};

const deleteKnowledge = {
  params: Joi.object().keys({
    knowledgeId: Joi.string().custom(objectId),
  }),
};

module.exports = {
  createKnowledge,
  getKnowledge,
  getKnowledgeDetail,
  updateKnowledge,
  deleteKnowledge,
};
