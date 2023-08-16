const httpStatus = require('http-status');
const { GarnerDescription } = require('../models');
const ApiError = require('../utils/ApiError');

/**
 * Create a user
 * @param {Object} newsBody
 * @returns {Promise<News>}
 */
module.exports.createGarnerDescription = async (descriptionBody) => {
  return GarnerDescription.create(descriptionBody);
};

module.exports.updateGarnerDescription = async (idToUpdate, updateBody) => {
  const garnerDescription = await GarnerDescription.findById(idToUpdate);
  //|| knowledge.trading_provider_id != updateBody.trading_provider_id
  if (!garnerDescription ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'GarnerDescription not found');
  }
  garnerDescription.status = updateBody.status
  garnerDescription.description = updateBody.description
  garnerDescription.descriptionContentType = updateBody.descriptionContentType
  await garnerDescription.save()
  return garnerDescription;
};


module.exports.updateGarnerDescriptionFeatures = async (idToUpdate, features) => {
  let garnerDescrition = await GarnerDescription.findById(idToUpdate)
  if (!garnerDescrition) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Garner Descrition not found');
  }
  delete features.__v;
  garnerDescrition.features = features;
  return garnerDescrition.save();
};

module.exports.updateGarnerDescriptionImages = async (idToUpdate, images) => {
  let garnerDescription = await GarnerDescription.findById(idToUpdate)
  if (!garnerDescription) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Garner Description not found');
  }
  garnerDescription.images = images.data;
  return garnerDescription.save();
};

module.exports.getGarnerDescription = async (tradingProviderId) => {
  return GarnerDescription.findOne({ trading_provider_id: tradingProviderId });
};

module.exports.getGarnerDescriptionByFilter = async (filter) => {
  return GarnerDescription.findOne(filter);
};


module.exports.getGarnerDescriptionMobile = async (filter) => {
  return GarnerDescription.findOne(filter);
};

module.exports.getGarnerDescriptionByTradingProviders = async (tradingProviderId) => {
  return GarnerDescription.findOne({ trading_provider_id: tradingProviderId });
};

module.exports.getGarnerDescriptions = async (tradingProviderIds) => {
  return GarnerDescription.find({ trading_provider_id: { $in: tradingProviderIds } });
};

