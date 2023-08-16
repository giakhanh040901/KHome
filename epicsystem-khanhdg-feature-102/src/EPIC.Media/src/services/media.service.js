const httpStatus = require('http-status');
const { Media, User } = require('../models');
const ApiError = require('../utils/ApiError');

/**
 * Create a user
 * @param {Object} mediaBody
 * @returns {Promise<Media>}
 */
const createMedia = async (mediaBody) => {
  return Media.create(mediaBody);
};

/**
 * Query for Media
 * @param {Object} filter - Mongo filter
 * @param {Object} options - Query options
 * @param {string} [options.sortBy] - Sort option in the format: sortField:(desc|asc)
 * @param {number} [options.limit] - Maximum number of results per page (default = 10)
 * @param {number} [options.page] - Current page (default = 1)
 * @returns {Promise<QueryResult>}
 */
const queryMedia = async (filter, options) => {
  // console.log("filter",filter);
  const medias = await Media.paginate(filter, options);
  return medias;
};

/**
 * Get Media by id
 * @param {ObjectId} id
 * @returns {Promise<User>}
 */
const getMediaById = async (id) => {
  return Media.findById(id);
};

/**
 * Delete Media by id
 * @param {ObjectId} MediaId
 * @returns {Promise<Media>}
 */
const deleteMediaById = async (mediaId) => {
  const media = await getMediaById(mediaId);
  if (!media) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Media not found');
  }
  await media.remove();
  return media;
};

/**
 * Update user by id
 * @param {ObjectId} mediaId
 * @param {Object} updateBody
 * @returns {Promise<Media>}
 */
const updateMediaById = async (mediaId, updateBody) => {
  const media = await getMediaById(mediaId);
  // if (!media || media.trading_provider_id != updateBody.trading_provider_id) {
  if (!media) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Media not found');
  }
  Object.assign(media, updateBody);
  await media.save();
  return media;
};

module.exports = {
  createMedia,
  queryMedia,
  getMediaById,
  updateMediaById,
  deleteMediaById,
};
