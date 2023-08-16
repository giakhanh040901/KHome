const httpStatus = require('http-status');
const pick = require('../utils/pick');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
const { notificationTemplateService, SystenNotification, notificationService } = require('../services');
const Chance = require('chance');
const { notificationStatus } = require("../config/newsEnums");
const chance = new Chance();

const createNotificationTemplate = catchAsync(async (req, res) => {
  req.body.trading_provider_id = req.user.trading_provider_id;
  req.body.creatorName = req.user.display_name;
  const notification = await notificationTemplateService.createNotificationTemplate(req.body);
  res.status(httpStatus.CREATED).send(notification);
});

const getNotificationTemplateList = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['title', 'q', 'status','typeModule']);
  if(req.tradingProvidersCanAccess) {
    filter.trading_provider_id = req.tradingProvidersCanAccess
  }
  if(req.user.isTrader) {
    filter.status = notificationStatus.ACTIVE
  }
  if(!filter.status) {
    filter.status = { $ne: notificationStatus.DELETED }
  }
  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "createdAt:desc";
  }
  if(filter.q) {
    filter.$or = [ {
      content: { "$regex": filter.q, "$options": "i" }
    }, {
      title: { "$regex": filter.q, "$options": "i" }
    },{
      code: { "$regex": filter.q, "$options": "i" }
    }]
    delete filter.q;
  } else {
    delete filter.q;
  }

  let query = {};
  if (filter.typeModule === 'eLoyalty') {
    query = { ...filter, typeModule: 'eLoyalty' };
  } else {
    query = { ...filter, typeModule: { $exists: false } };
  }

  const result = await notificationTemplateService.queryNotificationTemplates(query, options);

  for (let i = 0; i < result.results.length; i++) {
    const item = result.results[i];
    let filterNotification = {status: { $ne: notificationStatus.DELETED },trading_provider_id : req.tradingProvidersCanAccess}
    let optionsNotification = {limit:999999, page:1, select: ['notificationTemplateId','sentAPP','sentSMS','sentEmail']}
    let resultNotification = await notificationService.queryNotifications(filterNotification, optionsNotification);
    const promises = resultNotification.results.map(item => notificationService.getSendingListByNotification({ notification: item.id }, { limit: 999999, page: 1 }));

    const sendingLists = await Promise.all(promises);

    resultNotification.results.forEach((item, index) => {
      const sendingList = sendingLists[index];
      //tong so thong bao gui
      item.totalSending = sendingList?.totalResults ?? 0;
    });
    let maxVal  = 0
    result.results[i].usageCount = resultNotification.results.reduce((acc, cur) => {
      if (cur.notificationTemplateId === item.id) {
        maxVal = Math.max(...[cur.sentAPP, cur.sentEmail, cur.sentSMS]);
        acc += maxVal;
      }
      return acc;
    }, 0);
  }
  res.send(result);

});

const getNotificationTemplateDetail = catchAsync(async (req, res) => {
  const notificationTemplate = await notificationTemplateService.getNotificationTemplateById(req.params.notificationId);
  if (!notificationTemplate || (!req.user.isSuperAdmin && notificationTemplate.trading_provider_id != req.user.trading_provider_id)) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Notification Template not found');
  }
  res.send(notificationTemplate);
});

const updateNotificationTemplate = catchAsync(async (req, res) => {
  if(!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  const notificationTemplate = await notificationTemplateService.updateNotificationTemplateById(req.params.notificationTemplateId, req.body);
  res.send(notificationTemplate);
});

const deleteNotificationTemplate = catchAsync(async (req, res) => {
  await notificationTemplateService.deleteNotificationTemplateById(req.params.notificationTemplateId);
  res.status(httpStatus.NO_CONTENT).send();
});


module.exports = {
  createNotificationTemplate,
  getNotificationTemplateDetail,
  getNotificationTemplateList,
  updateNotificationTemplate,
  deleteNotificationTemplate
};
