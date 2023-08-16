const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');


const SentNotificationSchema = mongoose.Schema({
  userId: String,
  type: String,
  notification: mongoose.Schema.Types.Mixed,
  detail: String,
  title: String,
  isRead: {
    type: Boolean,
    default: false
  },
  isNavigation: String,
  navigationType: String,
  levelOneNavigation: String,
  secondLevelNavigation: String,
  navigationLink: String,
},
  {
  timestamps: true,
});

SentNotificationSchema.plugin(toJSON);
SentNotificationSchema.plugin(paginate);

const SentNotification = mongoose.model('SentNotification', SentNotificationSchema);

module.exports = SentNotification;
