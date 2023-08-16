const httpStatus = require('http-status');
const pick = require('../utils/pick');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
const { productImageService } = require('../services');
const { newsStatus } = require('../config/newsEnums');
const Chance = require('chance');
const chance = new Chance();


const createProductImage = catchAsync(async (req, res) => {
  // req.body.trading_provider_id = req.user.trading_provider_id;
  // req.body.user_type = req.user.user_type;
  req.body.createdBy = req.user.display_name;
  if(!req.body.productId) {
    //Nếu không phải là hình ảnh sản phẩm thì phải cho phép đại lý xem
    req.body.trading_provider_id = req.user.trading_provider_id;
    req.body.user_type = req.user.user_type;
  }
  if(req.user.user_type == 'E' || req.user.user_type == 'RE') {
    req.body.isRoot = true
  }
  req.body.code = chance.string({
    pool: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',
    length: 5
  });
  const productImage = await productImageService.createProductImage(req.body);
  res.status(httpStatus.CREATED).send(productImage);
});

const getProductImageList = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['title', 'displayText',  'q', 'type', 'productId', 'status']);

  if(filter.q) {
    filter.$or = [ {
      content: { "$regex": filter.q, "$options": "i" }
    }, {
      title: { "$regex": filter.q, "$options": "i" }
    },{
      code: { "$regex": filter.q, "$options": "i" }
    }]
    delete filter.q;
  }

  if(!filter.productId) {
    if(!req.user.isSuperAdmin) {
      filter.trading_provider_id = req.user.trading_provider_id
    }
  }

  if(req.user.isTrader) {
    filter.status = newsStatus.ACTIVE
  }

  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "createdAt:desc";
  }

  const result = await productImageService.queryProductImage(filter, options);
  res.send(result);
});

const getProductImageListMobile = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['title', 'displayText',  'q', 'type', 'position', 'status', 'productId']);
  if(filter.q) {
    filter.$or = [ {
      content: { "$regex": filter.q, "$options": "i" }
    }, {
      title: { "$regex": filter.q, "$options": "i" }
    }]
    delete filter.q;
  }

  if(req.tradingProvidersCanAccess) {
    filter.trading_provider_id = req.tradingProvidersCanAccess
  }else {
    filter.isRoot = true;
  }

  if(filter.productId) {
    delete filter.isRoot
    delete filter.q
    delete filter.trading_provider_id
    delete filter.$or
  }

  filter.status = newsStatus.ACTIVE
  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "createdAt:desc";
  }
  const result = await productImageService.queryProductImage(filter, options);
  res.send({
    status: 1,
    data: result,
    code: 200,
    message: 'OK'
  });
});

const getProductImageDetail = catchAsync(async (req, res) => {
  const productImage = await productImageService.getProductImageById(req.params.productImageId);
  if (!productImage ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'ProductImage not found');
  }
  res.send(productImage);
});

const getProductImageDetailMobile = catchAsync(async (req, res) => {
  const productImage = await productImageService.getProductImageById(req.params.productImageId);
  if (!productImage ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'ProductImage not found');
  }
  res.send({
    status: 1,
    data: productImage,
    code: 200,
    message: 'OK'
  });
});

const updateProductImage = catchAsync(async (req, res) => {
  if(!req.user.isSuperAdmin) {
    // req.body.trading_provider_id = req.user.trading_provider_id;
  }

  const ProductImage = await productImageService.updateProductImageById(req.params.productImageId, req.body);
  res.send(ProductImage);
});

const deleteProductImage = catchAsync(async (req, res) => {
  await productImageService.deleteProductImageById(req.params.productImageId);
  res.status(httpStatus.NO_CONTENT).send();
});

const uploadImages = catchAsync(async (req, res) => {
  const files = req.files
  if (!files) {
    const error = new Error('Please choose files')
    error.httpStatusCode = 400
    return next(error)
  }
  res.send(files)
});

module.exports = {
  createProductImage,
  getProductImageDetail,
  getProductImageDetailMobile,
  getProductImageList,
  getProductImageListMobile,
  updateProductImage,
  deleteProductImage,
  uploadImages
};
