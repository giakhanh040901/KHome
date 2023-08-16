const httpStatus = require('http-status');
const pick = require('../utils/pick');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
const { mediaService } = require('../services');
const { newsStatus } = require('../config/newsEnums');
const Chance = require('chance');
const { userType } = require("../config/roles");
const chance = new Chance();

const createMedia = catchAsync(async (req, res) => {
  req.body.trading_provider_id = req.user.trading_provider_id;
  req.body.user_type = req.user.user_type;
  req.body.createdBy = req.user.display_name;
  req.body.code = chance.string({
    pool: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',
    length: 5
  });
  if(req.user.user_type == 'E' || req.user.user_type == 'RE') {
    req.body.isRoot = true
  }

  const media = await mediaService.createMedia(req.body);
  res.status(httpStatus.CREATED).send(media);
});

const getMediaList = catchAsync(async (req, res) => {
  let query = {};

  const filter = pick(req.query, ['title', 'displayText',  'q', 'type', 'position', 'status', 'productKey','typeModule']);
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

  if(req.tradingProvidersCanAccess) {
    filter.trading_provider_id = req.tradingProvidersCanAccess
  }else {
    filter.isRoot = true;
  }

  const options = pick(req.query, ['sortBy', 'limit', 'page','Sort']);
  if(!options.sortBy) {
    options.sortBy = "approveAt:desc,createdAt:desc";
  }

  if (filter.typeModule === 'eLoyalty') {
    query = { ...filter, typeModule: 'eLoyalty' };
  } else {
    query = { ...filter, typeModule: { $exists: false } };
  }

  const result = await mediaService.queryMedia(query, options);

  res.send(result);
});

const getMediaListMobile = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['title', 'displayText',  'q', 'type', 'position', 'status',
    'productKey','navigationLink','isNavigation','navigationType','levelOneNavigation','secondLevelNavigation','typeModule']);
  if(filter.q) {
    filter.$or = [ {
      content: { "$regex": filter.q, "$options": "i" }
    }, {
      title: { "$regex": filter.q, "$options": "i" }
    }]
    delete filter.q;
  }

  const options = pick(req.query, ['sortBy', 'limit', 'page']);

  options.sortBy = "approveAt:desc,createdAt:desc";
  filter.status = 'ACTIVE';

  let query = {};
  //xu ly khi thuoc eloyalty (man voucher)
  if (filter.typeModule === 'eLoyalty') {
    filter.trading_provider_id = req.query.tradingProviderId;
    query = { ...filter}
  } else {

    //xu ly khi o man kham pha (home)
    if(req.tradingProvidersCanAccess) {
      filter.trading_provider_id = req.tradingProvidersCanAccess
    } else {
      filter.isRoot = true;
    }
    query = { ...filter, typeModule: { $ne: 'eLoyalty' } };
  }

  //code cu
  // if(req.tradingProvidersCanAccess) {
  //   filter.trading_provider_id = req.tradingProvidersCanAccess
  // } else {
  //   filter.isRoot = true;
  // }

  var result = await mediaService.queryMedia(query, options);

  res.send({
    status: 1,
    data: result,
    code: 200,
    message: 'OK'
  });
});

const getMediaDetail = catchAsync(async (req, res) => {
  const media = await mediaService.getMediaById(req.params.mediaId);
  if (!media ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Media not found');
  }
  res.send(media);
});

const getMediaDetailMobile = catchAsync(async (req, res) => {
  const media = await mediaService.getMediaById(req.params.mediaId);
  if (!media ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Media not found');
  }
  res.send({
    status: 1,
    data: media,
    code: 200,
    message: 'OK'
  });
});

const updateMedia = catchAsync(async (req, res) => {
  if(!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  // Nếu status là phê duyệt thì gán thêm trường người duyệt, ngày duyệt
  if (req.body.status === 'ACTIVE') {
    req.body.approveBy = req.user.display_name;
    req.body.approveAt = new Date();
  }
  const Media = await mediaService.updateMediaById(req.params.mediaId, req.body);
  res.send(Media);
});

const deleteMedia = catchAsync(async (req, res) => {
  await mediaService.deleteMediaById(req.params.mediaId);
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
  createMedia,
  getMediaDetail,
  getMediaDetailMobile,
  getMediaList,
  getMediaListMobile,
  updateMedia,
  deleteMedia,
  uploadImages
};
