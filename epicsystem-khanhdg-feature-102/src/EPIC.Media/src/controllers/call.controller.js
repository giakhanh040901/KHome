const httpStatus = require('http-status');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
const { callService } = require('../services');
const { request } = require('express');
const pick = require('../utils/pick');

const createCall = catchAsync(async (req, res) => {
  const newCall = await callService.saveCallInfo(req.body);
  res.status(httpStatus.OK).send(newCall);
});

const getCallList = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['q']);
  if (filter.q) {
    delete filter.q;
  }
  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  options.sortBy = 'startTime:desc';
  if (req.tradingProvidersCanAccess) {
    filter['receiver.trading_provider_id'] = req.tradingProvidersCanAccess;
  } else {
    filter['receiver.user_type'] = { $in: ['E', 'RE'] };
  }
  const result = await callService.queryCalls(filter, options);
  res.send({
    status: 1,
    data: result,
    code: 200,
    message: 'OK',
  });
});

module.exports = {
  createCall,
  getCallList,
};
