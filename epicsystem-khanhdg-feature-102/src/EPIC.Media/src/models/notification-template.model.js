const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');
const { notificationType, notificationActionType, contentType,
          newsStatus, navigationTypes, levelOneNavigationOptions } = require('../config/newsEnums');

const notificationTemplateSchema = mongoose.Schema(
  {
    code: String,
    title: {
      type: String,
      required: true
    },
    description: String,
    type: {
      type: String,
      enum: Object.values(notificationType),
      required: true,
      default: notificationType.TIN_TUC
    },
    contentType: {
      type: String,
      enum: Object.values(contentType),
      default: contentType.MARKDOWN
    },
    notificationContent: String,
    appNotificationDesc: String,
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
      enum: Object.values({
        ACTIVE : 'ACTIVE',
        INACTIVE: 'INACTIVE'
      }),
      required: true,
      default: newsStatus.DRAFT
    },
    user_type: String,
    actionView: String,
    externalEvent: String,
    externalParams: String,
    trading_provider_id: {
      type: Number,
      required: false
    },
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
    creatorName: String,
    usageCount: Number,
    typeModule: {
      type: String,
      required: false
    },
  },
  {
    timestamps: true
  },

);

// add plugin that converts mongoose to json
notificationTemplateSchema.plugin(toJSON);
notificationTemplateSchema.plugin(paginate);

const NotificationTemplate = mongoose.model('NotificationTemplate', notificationTemplateSchema);

module.exports = NotificationTemplate;
