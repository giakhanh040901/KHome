const httpStatus = require('http-status');
const { FacebookPost, User } = require('../models');
const ApiError = require('../utils/ApiError');

/**
 * Create a user
 * @param {Object} facebookPostBody
 * @returns {Promise<FacebookPost>}
 */
const createFacebookPost = async (facebookPostBody) => {
  return FacebookPost.create(facebookPostBody);
};

/**
 * Query for facebookPost
 * @param {Object} filter - Mongo filter
 * @param {Object} options - Query options
 * @param {string} [options.sortBy] - Sort option in the format: sortField:(desc|asc)
 * @param {number} [options.limit] - Maximum number of results per page (default = 10)
 * @param {number} [options.page] - Current page (default = 1)
 * @returns {Promise<QueryResult>}
 */
const queryFacebookPostAll = async (filter, options) => {
  const facebookPost = await FacebookPost.paginate(filter, options);
  return facebookPost;
};

const queryFacebookPost = async (filter) => {
  const facebookPost = await FacebookPost.find(filter);
  return facebookPost;
};
/**
 * Get facebookPost by id
 * @param {ObjectId} id
 * @returns {Promise<User>}
 */
const getFacebookPostById = async (id) => {
  return FacebookPost.findById(id);
};


const getFacebookPostByPostId = async (id, tradingProviderId) => {
  let filter = {id}
  if(!tradingProviderId) {
    filter.isRoot = true;
  }else {
    filter.trading_provider_id = tradingProviderId
  }
  return FacebookPost.findOne(filter);
};


/**
 * Delete facebookPost by id
 * @param {ObjectId} facebookPostId
 * @returns {Promise<FacebookPost>}
 */
const deleteFacebookPostById = async (facebookPostId) => {
  const facebookPost = await getFacebookPostById(facebookPostId);
  if (!facebookPost) {
    throw new ApiError(httpStatus.NOT_FOUND, 'FacebookPost not found');
  }
  await facebookPost.remove();
  return facebookPost;
};

/**
 * Update user by id
 * @param {ObjectId} userId
 * @param {Object} updateBody
 * @returns {Promise<User>}
 */
const updateFacebookPostById = async (facebookPostId, updateBody) => {
  const facebookPost = await getFacebookPostById(facebookPostId);
  //|| facebookPost.trading_provider_id != updateBody.trading_provider_id
  if (!facebookPost ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'FacebookPost not found');
  }
  Object.assign(facebookPost, updateBody);
  await facebookPost.save();
  return facebookPost;
};


const updateFacebookPostStatusById = async (facebookPostId, updateBody) => {
  const facebookPost = await getFacebookPostById(facebookPostId);
  //|| facebookPost.trading_provider_id != updateBody.trading_provider_id
  if (!facebookPost ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'FacebookPost not found');
  }
  facebookPost.status = updateBody.status
  await facebookPost.save();
  return facebookPost;
};

module.exports = {
  createFacebookPost,
  queryFacebookPost,
  getFacebookPostById,
  updateFacebookPostById,
  deleteFacebookPostById,
  getFacebookPostByPostId,
  queryFacebookPostAll,
  updateFacebookPostStatusById
};
