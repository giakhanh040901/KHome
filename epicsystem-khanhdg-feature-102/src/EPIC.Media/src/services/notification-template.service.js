const httpStatus = require('http-status');
const { NotificationTemplate, User } = require('../models');
const ApiError = require('../utils/ApiError');

/**
 * Create a user
 * @param {Object} notificationBody
 * @returns {Promise<Notification>}
 */
const createNotificationTemplate = async (notificationBody) => {
  return NotificationTemplate.create(notificationBody);
};

/**
 * Query for notification
 * @param {Object} filter - Mongo filter
 * @param {Object} options - Query options
 * @param {string} [options.sortBy] - Sort option in the format: sortField:(desc|asc)
 * @param {number} [options.limit] - Maximum number of results per page (default = 10)
 * @param {number} [options.page] - Current page (default = 1)
 * @returns {Promise<QueryResult>}
 */
const queryNotificationTemplates = async (filter, options) => {
  const notificationTemplates = await NotificationTemplate.paginate(filter, options);
  return notificationTemplates;
};

/**
 * Get notificationTemplate by id
 * @param {ObjectId} id
 * @returns {Promise<User>}
 */
const getNotificationTemplateById = async (id) => {
  return NotificationTemplate.findById(id);
};

/**
 * Delete notification by id
 * @param {ObjectId} notificationId
 * @returns {Promise<Notification>}
 */
const deleteNotificationTemplateById = async (notificationTemplateId) => {
  const notificationTemplate = await getNotificationTemplateById(notificationTemplateId);
  if (!notificationTemplate) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Notification Template not found');
  }
  await notificationTemplate.remove();
  return notificationTemplate;
};

/**
 * Update user by id
 * @param {ObjectId} userId
 * @param {Object} updateBody
 * @returns {Promise<User>}
 */
const updateNotificationTemplateById = async (notificationTemplateId, updateBody) => {
  const notificationTemplate = await getNotificationTemplateById(notificationTemplateId);
  //|| notification.trading_provider_id != updateBody.trading_provider_id
  if (!notificationTemplate ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Notification Template not found');
  }
  Object.assign(notificationTemplate, updateBody);
  await notificationTemplate.save();
  return notificationTemplate;
};

module.exports = {
  createNotificationTemplate,
  queryNotificationTemplates,
  getNotificationTemplateById,
  updateNotificationTemplateById,
  deleteNotificationTemplateById,
};
