const admin = require('firebase-admin');
const config = require('../config/config');

const serviceAccount = require('../../service-account-key.json');

admin.initializeApp({
  credential: admin.credential.cert(serviceAccount),
  databaseURL: 'https://epic-system-e245d.firebaseio.com',
});

const pushAppNotification = async (fcmToken = [], title, body, refId = '', image = '') => {
  console.log('zo');
  // for(var i = 0; i < fcmToken.length; i++) {
  // if(fcmToken.length > 0) {
  var message = {
    notification: {
      title,
      body,
    },
    tokens: [fcmToken[fcmToken.length - 1]],
    data: {
      id: refId.toString(),
    },
    apns: {
      headers: {
        'apns-priority': '10',
      },
      payload: {
        aps: {
          sound: 'default',
          badge: 0,
        },
      },
    },
    android: {
      priority: 'high',
      notification: {
        sound: 'default',
      },
    },
  };

  console.log('messsage', message);

  if (image && image != '') {
    message.notification.image = config.baseAPI.concat(image);
    message.android.notification.image = config.baseAPI.concat(image);
    message.aps = {};
    message.aps.fcm_options = {
      image: config.baseAPI.concat(image),
    };
    message.webpush = {
      headers: {
        image: config.baseAPI.concat(image),
      },
    };
  }
  console.log('APP_MESSAGE: ', message);
  let result = await admin.messaging().sendMulticast(message);
  if (result.successCount > 0) {
    return true;
  }
  console.log('FCM NOTIF: ', result);
  console.log('error: ', result.responses);
  return false;
  // }
  // return false;
};

module.exports = {
  pushAppNotification,
};
