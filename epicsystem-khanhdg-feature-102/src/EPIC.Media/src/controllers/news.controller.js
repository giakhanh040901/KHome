const httpStatus = require('http-status');
const pick = require('../utils/pick');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
const { newsService } = require('../services');
const Chance = require('chance');
const { newsStatus } = require("../config/newsEnums");
const chance = new Chance();

const createNews = catchAsync(async (req, res) => {
  req.body.trading_provider_id = req.user.trading_provider_id;
  req.body.createdBy = req.user.display_name;
  req.body.code = chance.string({
    pool: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',
    length: 5
  });
  if(req.user.user_type == 'E' || req.user.user_type == 'RE') {
    req.body.isRoot = true
  }

  const news = await newsService.createNews(req.body);
  res.status(httpStatus.CREATED).send(news);
});

const getNewsList = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['title', 'code', 'q', 'status','typeModule']);
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

const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "createdAt:desc";
  }
  let queryNews = {};
  if (filter.typeModule === 'eLoyalty') {
    queryNews = { ...filter, typeModule: 'eLoyalty' };
  } else {
    queryNews = { ...filter, typeModule: { $exists: false } };
  }
  const result = await newsService.queryNews(queryNews, options);
  res.send(result);

});

const getNewsListMobile = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['title', 'code', 'q', 'status','typeModule']);

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
  filter.status = newsStatus.ACTIVE
  const options = pick(req.query, ['sortBy', 'limit', 'page']);

  options.sortBy = "approveAt:desc,createdAt:desc";

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

  const result = await newsService.queryNews(query, options);
  res.send({
    status: 1,
    data: result,
    code: 200,
    message: 'OK'
  });
});

const getNewsDetail = catchAsync(async (req, res) => {
  const news = await newsService.getNewsById(req.params.newsId);
  if (!news ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'News not found');
  }
  res.send(news);
});

const getNewsDetailMobile = catchAsync(async (req, res) => {
  const news = await newsService.getNewsById(req.params.newsId);
  if (!news ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'News not found');
  }
  res.send({
    status: 1,
    data: news,
    code: 200,
    message: 'OK'
  });
});

const updateNews = catchAsync(async (req, res) => {
  if(!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  // Nếu status là phê duyệt thì gán thêm trường người duyệt, ngày duyệt
  if (req.body.status === 'ACTIVE') {
    req.body.approveBy = req.user.display_name;
    req.body.approveAt = new Date();
  }
  const news = await newsService.updateNewsById(req.params.newsId, req.body);
  res.send(news);
});

const deleteNews = catchAsync(async (req, res) => {
  await newsService.deleteNewsById(req.params.newsId);
  res.status(httpStatus.NO_CONTENT).send();
});

module.exports = {
  createNews,
  getNewsDetail,
  getNewsDetailMobile,
  getNewsList,
  getNewsListMobile,
  updateNews,
  deleteNews,
};
