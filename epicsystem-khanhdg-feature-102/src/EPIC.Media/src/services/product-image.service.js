const httpStatus = require('http-status');
const { ProductImage, User } = require('../models');
const ApiError = require('../utils/ApiError');

/**
 * Create a user
 * @param {Object} productImageBody
 * @returns {Promise<ProductImage>}
 */
const createProductImage = async (productImageBody) => {
  return ProductImage.create(productImageBody);
};

/**
 * Query for ProductImage
 * @param {Object} filter - Mongo filter
 * @param {Object} options - Query options
 * @param {string} [options.sortBy] - Sort option in the format: sortField:(desc|asc)
 * @param {number} [options.limit] - Maximum number of results per page (default = 10)
 * @param {number} [options.page] - Current page (default = 1)
 * @returns {Promise<QueryResult>}
 */
const queryProductImage = async (filter, options) => {
  const productImages = await ProductImage.paginate(filter, options);
  return productImages;
};

/**
 * Get ProductImage by id
 * @param {ObjectId} id
 * @returns {Promise<User>}
 */

const getProductImageById = async (id) => {
  return ProductImage.findById(id);
};

/**
 * Delete ProductImage by id
 * @param {ObjectId} ProductImageId
 * @returns {Promise<ProductImage>}
 */
const deleteProductImageById = async (productImageId) => {
  const productImage = await getProductImageById(productImageId);
  if (!productImage) {
    throw new ApiError(httpStatus.NOT_FOUND, 'ProductImage not found');
  }
  await productImage.remove();
  return productImage;
};

/**
 * Update user by id
 * @param {ObjectId} productImageId
 * @param {Object} updateBody
 * @returns {Promise<ProductImage>}
 */
const updateProductImageById = async (productImageId, updateBody) => {
  const productImage = await getProductImageById(productImageId);
  // if (!productImage || productImage.trading_provider_id != updateBody.trading_provider_id) {
  if (!productImage) {
    throw new ApiError(httpStatus.NOT_FOUND, 'ProductImage not found');
  }
  Object.assign(productImage, updateBody);
  await productImage.save();
  return productImage;
};

module.exports = {
  createProductImage,
  queryProductImage,
  getProductImageById,
  updateProductImageById,
  deleteProductImageById,
};
