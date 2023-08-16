const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');
const { notificationType, notificationActionType, contentType,
          newsStatus, navigationTypes, levelOneNavigationOptions, sentStatus } = require('../config/newsEnums');

// de tam
const cron = require('cron');
// const { notificationService  } = require('../services');
//

const notificationSchema = mongoose.Schema(
  {
    title: {
      type: String,
      required: true
    },
    description: String,
    type: {
      type: String,
      enum: Object.values(notificationType),
      required: true,
      default: notificationType.UU_DAI
    },
    emailContentType: {
      type: String,
      enum: Object.values(contentType),
      default: contentType.MARKDOWN
    },
    notificationContent: String,
    appNotificationDesc: String,
    appNotifContentType: {
      type: String,
      enum: Object.values(contentType),
      default: contentType.MARKDOWN
    },
    smsContent: String,
    emailContent: String,
    mainImg: String,
    actions: [
      {
        type: String,
        enum: Object.values(notificationActionType)
      }
    ],
    isFeatured: {
      type: Boolean,
      default: false
    },
    allowShare: {
      type: Boolean,
      default: false
    },
    status: {
      type: String,
      enum: Object.values(newsStatus),
      required: true,
      default: newsStatus.DRAFT
    },
    trading_provider_id: Number,
    creatorName: String,
    user_type: String,
    isNavigation: {
      type: Boolean,
      default: false
    },
    navigationType: {
      type: String,
      enum: Object.values(navigationTypes),
      default: navigationTypes.NULL,
    },
    levelOneNavigation: {
      type: String,
      enum: Object.values(levelOneNavigationOptions),
      default: levelOneNavigationOptions.NULL,
    },
    secondLevelNavigation: {
      type: String,
      required: false
    },
    navigationLink: String,
    isTimer: {
      type: Boolean,
      default: false
    },
    appointment: Date,
    notificationTemplateId: {
      type: String,
      required: false
    },
    statusSent: {
      type: String,
      default: sentStatus.DRAFT
    },
    totalSending: Number,
    sentEmail: Number,
    sentSMS: Number,
    sentAPP: Number,
    dateSent: Date,
    typeModule: {
      type: String,
      required: false
    },
  },
  {
    timestamps: true,
  });

// add plugin that converts mongoose to json
notificationSchema.plugin(toJSON);
notificationSchema.plugin(paginate);

const Notification = mongoose.model('Notification', notificationSchema);

module.exports = Notification;
