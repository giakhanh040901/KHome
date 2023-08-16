const mongoose = require('mongoose');
const { toJSON } = require('./plugins');

const configSchema = mongoose.Schema(
  {
    trading_provider_id: Number,
    smsConfig: {
      USERNAME:String,
      PASSWORD: String,
      BRANDNAME: String
    },
    stmpConfig: {
      SMTP_HOST: String,
      SMTP_PORT: String,
      SMTP_USERNAME: String,
      SMTP_PASSWORD: String,
      EMAIL_FROM: String,
      EMAIL_BRAND_NAME: String
    },
    pushAppTitle: String
  },
  {
    timestamps: true,
  });

configSchema.plugin(toJSON);
const NotificationConfig = mongoose.model('NotificationConfigSchema', configSchema);

module.exports = NotificationConfig;
