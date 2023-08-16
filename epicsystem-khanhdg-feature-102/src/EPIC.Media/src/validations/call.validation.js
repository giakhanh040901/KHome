const Joi = require('joi');
const { objectId } = require('./custom.validation');

const createCallInfo = {
    body: Joi.object().keys({
      caller: Joi.object().required(),
      receiver: Joi.object().required(),
      status:  Joi.string().required().valid('SUCCESS', 'MISSING'),
      startTime: Joi.date().required(),
      endTime: Joi.date(),
      duration: Joi.string(),
      urlAudio: Joi.string()
    }),
  };

  const getHistoryCall = {
    query: Joi.object().keys({
      sortBy: Joi.string(),
      q: Joi.string(),
      limit: Joi.number().integer(),
      page: Joi.number().integer(),
    }).unknown(true)
  };

  module.exports = {
    createCallInfo,
    getHistoryCall
  };
