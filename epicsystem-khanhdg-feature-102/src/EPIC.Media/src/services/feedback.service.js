const httpStatus = require('http-status');
const { Feedback, User } = require('../models');
const ApiError = require('../utils/ApiError');

/**
 * Create a user
 * @param {Object} feedbackBody
 * @returns {Promise<Feedback>}
 */
const createFeedback = async (feedbackBody) => {
  return Feedback.create(feedbackBody);
};

/**
 * Query for feedback
 * @param {Object} filter - Mongo filter
 * @param {Object} options - Query options
 * @param {string} [options.sortBy] - Sort option in the format: sortField:(desc|asc)
 * @param {number} [options.limit] - Maximum number of results per page (default = 10)
 * @param {number} [options.page] - Current page (default = 1)
 * @returns {Promise<QueryResult>}
 */
const queryFeedback = async (filter, options) => {
  const feedback = await Feedback.paginate(filter, options);
  return feedback;
};

/**
 * Get feedback by id
 * @param {ObjectId} id
 * @returns {Promise<User>}
 */
const getFeedbackById = async (id) => {
  return Feedback.findById(id);
};

/**
 * Delete feedback by id
 * @param {ObjectId} feedbackId
 * @returns {Promise<Feedback>}
 */
const deleteFeedbackById = async (feedbackId) => {
  const feedback = await getFeedbackById(feedbackId);
  if (!feedback) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Feedback not found');
  }
  await feedback.remove();
  return feedback;
};

/**
 * Update user by id
 * @param {ObjectId} userId
 * @param {Object} updateBody
 * @returns {Promise<User>}
 */
const updateFeedbackById = async (feedbackId, updateBody) => {
  const feedback = await getFeedbackById(feedbackId);
  //|| feedback.trading_provider_id != updateBody.trading_provider_id
  if (!feedback ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Feedback not found');
  }
  Object.assign(feedback, updateBody);
  await feedback.save();
  return feedback;
};

module.exports = {
  createFeedback,
  queryFeedback,
  getFeedbackById,
  updateFeedbackById,
  deleteFeedbackById,
};
