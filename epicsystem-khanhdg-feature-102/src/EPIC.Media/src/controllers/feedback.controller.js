const httpStatus = require('http-status');
const pick = require('../utils/pick');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
const { feedbackService } = require('../services');
const Chance = require('chance');
const chance = new Chance();

const createFeedback = catchAsync(async (req, res) => {
  // req.body.code = chance.string({
  //   pool: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',
  //   length: 5
  // });
  if(req.user.user_type == 'E') {
    req.body.isRoot = true
  }

  req.body.trading_provider_id = req.user.trading_provider_id;
  req.body.senderName = req.user.email;
  req.body.senderId = req.user.investor_id;
  const feedback = await feedbackService.createFeedback(req.body);
  res.status(httpStatus.CREATED).send(feedback);
});

const getFeedbackList = catchAsync(async (req, res) => {
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

  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "createdAt:desc";
  }

  const result = await feedbackService.queryFeedback(filter, options);
  res.send(result);
});

const getFeedbackListMobile = catchAsync(async (req, res) => {
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

  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "createdAt:desc";
  }
  if(filter.q) {
    filter.content = { "$regex": filter.q, "$options": "i" }
  }
  filter.senderId = req.user.investor_id;
  const result = await feedbackService.queryFeedback(filter, options);
  res.send({
    status: 1,
    data: result,
    code: 200,
    message: 'OK'
  });
});

const getFeedbackDetail = catchAsync(async (req, res) => {
  const feedback = await feedbackService.getFeedbackById(req.params.feedbackId);
  if (!feedback) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Feedback not found');
  }
  res.send(feedback);
});

const getFeedbackDetailMobile = catchAsync(async (req, res) => {
  const feedback = await feedbackService.getFeedbackById(req.params.feedbackId);
  if (!feedback) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Feedback not found');
  }
  res.send({
    status: 1,
    data: feedback,
    code: 200,
    message: 'OK'
  });
});

const updateFeedback = catchAsync(async (req, res) => {
  if(!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  const feedback = await feedbackService.updateFeedbackById(req.params.feedbackId, req.body);
  res.send(feedback);
});

const deleteFeedback = catchAsync(async (req, res) => {
  await feedbackService.deleteFeedbackById(req.params.feedbackId);
  res.status(httpStatus.NO_CONTENT).send();
});
;;


module.exports = {
  createFeedback,
  getFeedbackDetail,
  getFeedbackDetailMobile,
  getFeedbackListMobile,
  getFeedbackList,
  updateFeedback,
  deleteFeedback,
};
