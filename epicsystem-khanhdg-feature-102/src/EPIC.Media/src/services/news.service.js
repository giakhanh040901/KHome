const httpStatus = require('http-status');
const { News, User } = require('../models');
const ApiError = require('../utils/ApiError');

/**
 * Create a user
 * @param {Object} newsBody
 * @returns {Promise<News>}
 */
const createNews = async (newsBody) => {
  return News.create(newsBody);
};

/**
 * Query for news
 * @param {Object} filter - Mongo filter
 * @param {Object} options - Query options
 * @param {string} [options.sortBy] - Sort option in the format: sortField:(desc|asc)
 * @param {number} [options.limit] - Maximum number of results per page (default = 10)
 * @param {number} [options.page] - Current page (default = 1)
 * @returns {Promise<QueryResult>}
 */
const queryNews = async (filter, options) => {
  const news = await News.paginate(filter, options);
  return news;
};

/**
 * Get news by id
 * @param {ObjectId} id
 * @returns {Promise<User>}
 */
const getNewsById = async (id) => {
  return News.findById(id);
};

/**
 * Delete news by id
 * @param {ObjectId} newsId
 * @returns {Promise<News>}
 */
const deleteNewsById = async (newsId) => {
  const news = await getNewsById(newsId);
  if (!news) {
    throw new ApiError(httpStatus.NOT_FOUND, 'News not found');
  }
  await news.remove();
  return news;
};

/**
 * Update user by id
 * @param {ObjectId} userId
 * @param {Object} updateBody
 * @returns {Promise<User>}
 */
const updateNewsById = async (newsId, updateBody) => {
  const news = await getNewsById(newsId);
  //|| news.trading_provider_id != updateBody.trading_provider_id
  if (!news ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'News not found');
  }
  Object.assign(news, updateBody);
  await news.save();
  return news;
};

module.exports = {
  createNews,
  queryNews,
  getNewsById,
  updateNewsById,
  deleteNewsById,
};
