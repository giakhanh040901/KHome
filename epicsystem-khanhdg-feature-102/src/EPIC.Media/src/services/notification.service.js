const httpStatus = require('http-status');
const { Notification, User, SendingList, SystenNotification, SentNotification } = require('../models');
const ApiError = require('../utils/ApiError');

/**
 * Create a user
 * @param {Object} notificationBody
 * @returns {Promise<Notification>}
 */
const createNotification = async (notificationBody) => {
  return Notification.create(notificationBody);
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
const queryNotifications = async (filter, options) => {
  const notifications = await Notification.paginate(filter, options);
  return notifications;
};

/**
 * Get notification by id
 * @param {ObjectId} id
 * @returns {Promise<User>}
 */
const getNotificationById = async (id) => {
  return Notification.findById(id);
};

const getPersonInList = async (id) => {
  return SendingList.findById(id);
};

const getSentNotificationById = async (id) => {
  return SentNotification.findById(id);
};

/**
 * Delete notification by id
 * @param {ObjectId} notificationId
 * @returns {Promise<Notification>}
 */
const deleteNotificationById = async (notificationId) => {
  const notification = await getNotificationById(notificationId);
  if (!notification) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Notification not found');
  }
  await notification.remove();
  return notification;
};

/**
 * Update user by id
 * @param {ObjectId} userId
 * @param {Object} updateBody
 * @returns {Promise<User>}
 */
const updateNotificationById = async (notificationId, updateBody) => {
  const notification = await getNotificationById(notificationId);
  //|| notification.trading_provider_id != updateBody.trading_provider_id
  if (!notification ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Notification not found');
  }
  Object.assign(notification, updateBody);
  await notification.save();
  return notification;
};

const updateNotificationSentCounts = async (notificationId, sentEmailCount, sentSMSCount, sentAppCount) => {
  try {
    const updatedNotification = await NotificationModel.findByIdAndUpdate(
      notificationId,
      {
        sentEmail: sentEmailCount,
        sentSMS: sentSMSCount,
        sentAPP: sentAppCount,
      },
      { new: true }
    );
    return updatedNotification;
  } catch (error) {
    throw new Error(`Could not update notification with ID ${notificationId}: ${error.message}`);
  }
};

const querySentUserNotification = async (filter, options) => {
  const notifications = await SentNotification.paginate(filter, options);
  notifications.readCount = await SentNotification.aggregate(
    [
      {
        $match: {
          userId: filter.userId,
          isRead: false
        }
      },
      {
        $group : { _id : '$isRead', count : {$sum : 1}}
      }
    ]
  )
  notifications.count = await SentNotification.aggregate(
    [
      {
        $match: {
          userId: filter.userId,
          isRead: false
        }
      },
      {$group : { _id : '$type', count : {$sum : 1}}}
    ]
  )
  return notifications;
};

const setNotificationReadById = async (notificationId) => {
  const sentNotification = await SentNotification.findById(notificationId);
  //|| notification.trading_provider_id != updateBody.trading_provider_id
  if (!sentNotification ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Sent Notification not found');
  }
  sentNotification.isRead = true;
  await sentNotification.save();
  return sentNotification;
};

const updateSendingList = async (notificationId, sendingList, trading_provider_id) => {
  const notification = await getNotificationById(notificationId);
  await emptySendingList(notificationId)
  if (!notification ) {
    throw new ApiError(httpStatus.NOT_FOUND, 'Notification not found');
  }
  let sendingListWithNotification = sendingList.map(person => {
    if(!notification.actions.includes('SEND_EMAIL')) {
      person.sendEmailStatus = "NOT_AVAILABLE"
    }
    if(!notification.actions.includes('SEND_SMS')) {
      person.sendSMSStatus = "NOT_AVAILABLE"
    }
    if(!notification.actions.includes('PUSH_NOTIFICATION')) {
      person.pushAppStatus = "NOT_AVAILABLE"
    }
    person.notification = notification.id;
    person.trading_provider_id = trading_provider_id;
    return person;
  })

  return SendingList.insertMany(sendingListWithNotification);
};

const getSendingListByNotification = async (filter, options) => {

  return SendingList.paginate(filter, options);
};

const getSendingListByNotificationId = async (notificationId) => {
  const sendingList = await SendingList.find({
    notification: notificationId
  });
  return sendingList;
};

const createOrUpdateSystemNotification = async(tradingProviderId, settings, type) => {
  let filter = {
    trading_provider_id: tradingProviderId,
    type
  }
  if(type === 'default') {
    filter = {
      trading_provider_id: 0
    }
    tradingProviderId = 0;
  }
  const systemNotification = await SystenNotification.findOne({
    trading_provider_id: tradingProviderId,
    type: type
  });

  console.log("systemNotification",type);

  if(systemNotification) {
    systemNotification.settings = settings;
    await systemNotification.save();
    return systemNotification;
  }else {
    return SystenNotification.create({
      trading_provider_id: tradingProviderId,
      settings: settings,
      type
    });
  }
}

const getSystemNotificationByPartnerId = async (trading_provider_id, type) => {
  return SystenNotification.findOne({
    trading_provider_id: trading_provider_id,
    type
  });
};

const getAllSystemNotificationByPartnerId = async (trading_provider_id) => {
  return SystenNotification.find({
    trading_provider_id: trading_provider_id
  });
};

const getNewSystemNotificationByPartnerId = async (trading_provider_id) => {
  return SystenNotification.find({ trading_provider_id: trading_provider_id })
    .sort({ updatedAt: -1 })
    .limit(1);
};


const deleteManyPersonIds = async (ids) => {
  return SendingList.deleteMany({
    _id: { $in: ids }
  })
}

const emptySendingList = async (notificationId) => {
  return SendingList.deleteMany({
    notification: notificationId
  });
}


const deleteAllNotificationbyUserId = async (userId) => {
  return SentNotification.deleteMany({
    userId: userId
  })
}

const deleteAllNotificationbyId = async (id) => {
  return SentNotification.deleteOne({
    _id: id
  })
}

const readAllNotificationbyUserId = async (userId) => {
  return SentNotification.updateMany({
    userId: userId
  }, {
    isRead: true
  })
}

const updateIsLock = async (receiverId, isLock) => {
  try {
    const updatedReceiver = await SendingList.findOneAndUpdate(
      { _id: receiverId },
      { isLock },
      { new: true }
    );
    return updatedReceiver;
  } catch (error) {
    throw error;
  }
};

const updateIsLockBulk = async (receiverIds, isLock) => {
  try {
    const updatedReceiver = await SendingList.updateMany(
      { _id: { $in: receiverIds } },
      { $set: { isLock: isLock } }
    );
    return updatedReceiver;
  } catch (error) {
    throw error;
  }
};

module.exports = {
  createNotification,
  queryNotifications,
  getNotificationById,
  updateNotificationById,
  deleteNotificationById,
  updateSendingList,
  getSendingListByNotification,
  createOrUpdateSystemNotification,
  getSystemNotificationByPartnerId,
  getPersonInList,
  deleteManyPersonIds,
  getSentNotificationById,
  querySentUserNotification,
  setNotificationReadById,
  deleteAllNotificationbyUserId,
  readAllNotificationbyUserId,
  getAllSystemNotificationByPartnerId,
  deleteAllNotificationbyId,
  getSendingListByNotificationId,
  updateNotificationSentCounts,
  updateIsLock,
  updateIsLockBulk,
  getNewSystemNotificationByPartnerId
};
