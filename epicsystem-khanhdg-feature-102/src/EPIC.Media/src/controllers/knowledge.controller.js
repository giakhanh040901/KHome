const httpStatus = require('http-status');
const pick = require('../utils/pick');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
const { knowledgeService } = require('../services');
const Chance = require('chance');
const chance = new Chance();

const createKnowledge = catchAsync(async (req, res) => {
  // req.body.code = chance.string({
  //   pool: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',
  //   length: 5
  // });
  if(req.user.user_type == 'E' || req.user.user_type == 'RE') {
    req.body.isRoot = true
  }

  req.body.trading_provider_id = req.user.trading_provider_id;
  req.body.createdBy = req.user.display_name;
  const knowledge = await knowledgeService.createKnowledge(req.body);
  res.status(httpStatus.CREATED).send(knowledge);
});

const getKnowledgeList = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['title', 'code', 'q', 'status', 'category']);
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
  }
  const options = pick(req.query, ['sortBy', 'limit', 'page']);

  if(!options.sortBy) {
   options.sortBy = "approveAt:desc,createdAt:desc";
  }

  if(req.tradingProvidersCanAccess) {
    filter.trading_provider_id = req.tradingProvidersCanAccess
  }else {
    filter.isRoot = true;
  }

  const result = await knowledgeService.queryKnowledge(filter, options);
  res.send(result);
});

const getKnowledgeListMobile = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['title', 'code', 'q', 'status']);
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
  /**
   * Comment ngày 29, sau bỏ đi
   */
  if(req.tradingProvidersCanAccess) {
    filter.trading_provider_id = req.tradingProvidersCanAccess
  }else {
    filter.isRoot = true;
  }
  /**
   * Kết thúc comment
   */
  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  // if(!options.sortBy) {
  //
  // }
  options.sortBy = "approveAt:desc,createdAt:desc";
  if(filter.q) {
    filter.content = { "$regex": filter.q, "$options": "i" }
  }
  filter.status = 'ACTIVE';
  const result = await knowledgeService.queryKnowledge(filter, options);
  res.send({
    status: 1,
    data: result,
    code: 200,
    message: 'OK'
  });
});

const getKnowledgeDetail = catchAsync(async (req, res) => {
  const knowledge = await knowledgeService.getKnowledgeById(req.params.knowledgeId);
  if (!knowledge) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Knowledge not found');
  }
  res.send(knowledge);
});

const getKnowledgeDetailMobile = catchAsync(async (req, res) => {
  const knowledge = await knowledgeService.getKnowledgeById(req.params.knowledgeId);
  if (!knowledge) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Knowledge not found');
  }
  res.send({
    status: 1,
    data: knowledge,
    code: 200,
    message: 'OK'
  });
});

const updateKnowledge = catchAsync(async (req, res) => {
  if(!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  // Nếu status là phê duyệt thì gán thêm trường người duyệt, ngày duyệt
  if (req.body.status === 'ACTIVE') {
    req.body.approveBy = req.user.display_name;
    req.body.approveAt = new Date();
  }
  const knowledge = await knowledgeService.updateKnowledgeById(req.params.knowledgeId, req.body);
  res.send(knowledge);
});

const deleteKnowledge = catchAsync(async (req, res) => {
  await knowledgeService.deleteKnowledgeById(req.params.knowledgeId);
  res.status(httpStatus.NO_CONTENT).send();
});
;;


module.exports = {
  createKnowledge,
  getKnowledgeDetail,
  getKnowledgeDetailMobile,
  getKnowledgeListMobile,
  getKnowledgeList,
  updateKnowledge,
  deleteKnowledge,
};
