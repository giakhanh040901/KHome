const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');
const { newsStatus } = require('../config/newsEnums');

const statuses = ['NOT_AVAILABLE','DRAFT', 'SENDING', 'SENT', 'SEND_ERROR']

const sendingSchema = mongoose.Schema(
  {
    fullName: {
      type: String,
      require: false
    },
    personCode: String,
    fcmTokens: [String],
    phoneNumber: String,
    email: String,
    pushAppStatus: {
      type: String,
      enum: statuses,
      default: "DRAFT"
    },
    sendSMSStatus: {
      type: String,
      enum: statuses,
      default: "DRAFT"
    },
    sendEmailStatus: {
      type: String,
      enum: statuses,
      default: "DRAFT"
    },
    notification: {
      type: mongoose.Schema.Types.ObjectId,
      ref: 'Notification',
      require: true
    },
    logs: [{
      date: {
        type: Date,
        // `Date.now()` returns the current unix timestamp as a number
        default: Date.now
      },
      type: {
        type: String,
        enum: ['SMS', 'APP', 'EMAIL']
      },
      logMessage: String
    }],
    smsError: mongoose.Schema.Types.Mixed,
    appError: mongoose.Schema.Types.Mixed,
    emailError: mongoose.Schema.Types.Mixed,
    isLock: {
      type: Boolean,
      default: false
    },
  },
  {
    timestamps: true,
  });

// add plugin that converts mongoose to json
sendingSchema.plugin(toJSON);
sendingSchema.plugin(paginate);

const SendingList = mongoose.model('SendingList', sendingSchema);

module.exports = SendingList;
