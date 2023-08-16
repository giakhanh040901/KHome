const Joi = require("joi");


module.exports.updateGarnerProductImages = {
  body: Joi.object().keys({
    data: Joi.array().items(Joi.object({
      id: Joi.number(),
      order: Joi.number(),
      position: Joi.string(),
      path: Joi.string(),
      status: Joi.string(),
      event: Joi.object({
        screen: Joi.string().allow(''),
        params: Joi.object().allow('')
      })
    })),
    idToUpdate: Joi.string()
  }).unknown(true)
};

module.exports.updateGarnerProductFeatures = {
  body: Joi.object().keys({
    data: Joi.array().items(Joi.object({
      order: Joi.number(),
      iconUri: Joi.string(),
      description: Joi.string(),
      fileUrl: Joi.string(),
      status: Joi.string().allow('').allow(null),
      type: Joi.string(),
    })),
    idToUpdate: Joi.string()
  }).unknown(true)
};

module.exports.updateGarnerProductDescription = {
  body: Joi.object().keys({
    status: Joi.string(),
    description: Joi.string().allow(''),
    descriptionContentType: Joi.string().allow('')
  }).unknown(true)
}

