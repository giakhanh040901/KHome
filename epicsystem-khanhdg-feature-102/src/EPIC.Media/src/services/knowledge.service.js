const httpStatus = require('http-status');
const { Knowledge, User } = require('../models');
const ApiError = require('../utils/ApiError');

/**
 * Create a user
 * @param {Object} knowledgeBody
 * @returns {Promise<Knowledge>}
 */
const createKnowledge = async (knowledgeBody) => {
  return Knowledge.create(knowledgeBody);
};

/**
 * Query for knowledge
 * @param {Object} filter - Mongo filter
 * @param {Object} options - Query options
 * @param {string} [options.sortBy] - Sort option in the format: sortField:(desc|asc)
 * @param {number} [options.limit] - Maximum number of results per page (default = 10)
 * @param {number} [options.page] - Current page (default = 1)
 * @returns {Promise<QueryResult>}
 */
const queryKnowledge = async (filter, options) => {
  const knowledge = await Knowledge.paginate(filter, options);
  return knowledge;
};

/**
 * Get knowledge by id
 * @param {ObjectId} id
 * @returns {Promise<User>}
 */
const getKnowledgeById = async (id) => {
  return Knowledge.findById(id);
};

/**
 * Delete knowledge by id
 * @param {ObjectId} knowledgeId
 * @returns {Promise<Knowledge>}
 */
const deleteKnowledgeById = async (knowledgeId) => {
  const knowledge = await getKnowledgeById(knowledgeId);
  if (!knowledge) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Knowledge not found');
  }
  await knowledge.remove();
  return knowledge;
};

/**
 * Update user by id
 * @param {ObjectId} userId
 * @param {Object} updateBody
 * @returns {Promise<User>}
 */
const updateKnowledgeById = async (knowledgeId, updateBody) => {
  const knowledge = await getKnowledgeById(knowledgeId);
  //|| knowledge.trading_provider_id != updateBody.trading_provider_id
  if (!knowledge ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Knowledge not found');
  }
  Object.assign(knowledge, updateBody);
  await knowledge.save();
  return knowledge;
};

module.exports = {
  createKnowledge,
  queryKnowledge,
  getKnowledgeById,
  updateKnowledgeById,
  deleteKnowledgeById,
};
