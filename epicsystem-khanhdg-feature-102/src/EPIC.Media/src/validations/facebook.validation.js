const Joi = require('joi');
const { objectId } = require('./custom.validation');

const exchangeTokenValidation = {
  body: Joi.object().keys({
    token: Joi.string().required()
  }),
};

const addFacebookPostValidation = {
  body: Joi.object().keys({
    message: Joi.string().required(),
    created_time: Joi.string().required(),
    updated_time: Joi.string().required(),
    permalink_url: Joi.string().required()
  }).unknown(true),
};



const updatePost = {
  params: Joi.object().keys({
    postId: Joi.required().custom(objectId),
  }),
  body: Joi.object().keys({
    message: Joi.string().required(),
    permalink_url: Joi.string().required()
  }).unknown(true)
};

const updatePostStatus = {
  params: Joi.object().keys({
    postId: Joi.required().custom(objectId),
  }),
  body: Joi.object().keys({
    status: Joi.string().required()
  }).unknown(true)
};

module.exports = {
  exchangeTokenValidation,
  addFacebookPostValidation,
  updatePost,
  updatePostStatus
};
