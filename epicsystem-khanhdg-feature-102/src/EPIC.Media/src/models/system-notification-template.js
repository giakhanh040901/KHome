const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');
const { notificationActionType, systemNotificationTemplateKey, contentType } = require('../config/newsEnums');

const systemNotificationTemplateSchema = mongoose.Schema({
  key: {
    type: String,
    required: true
  },
  notificationTemplateName: {
    type: String,
    required: true
  },
  description: String,
  actions: [
    {
      type: String,
      enum: Object.values(notificationActionType)
    }
  ],
  emailContentType: {
    type: String,
    enum: Object.values(contentType),
    default: contentType.MARKDOWN
  },
  pushAppContentType: {
  type: String,
  enum: Object.values(contentType),
  default: contentType.MARKDOWN
  },
  smsContentType: {
    type: String,
    enum: Object.values(contentType),
    default: contentType.MARKDOWN
  },
  isAdmin: {
    type: Boolean,
    default: false
  },
  emailContent: String,
  titleAppContent: String,
  pushAppContent: String,
  smsContent: String,
  adminEmail: [String],
  adminPhone: String,
})

const tradingProviderSystemNotificationTemplate = mongoose.Schema(
  {
    settings: [systemNotificationTemplateSchema],
    trading_provider_id: {
      type: Number,
      required: false
    },
    type: {
      type: String,
      enum: ['system', 'bond', 'real_estate_invest', 'default', 'garner', 'real_estate','loyalty','event']
    }
  },
  {
    timestamps: true,
  });

// add plugin that converts mongoose to json
tradingProviderSystemNotificationTemplate.plugin(toJSON);
tradingProviderSystemNotificationTemplate.plugin(paginate);

const SystemNotificationTemplate = mongoose.model('SystemNotificationTemplate', tradingProviderSystemNotificationTemplate);

module.exports = SystemNotificationTemplate;
