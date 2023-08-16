  const admin = require("firebase-admin");
  // const config = require('../config/config');

  const serviceAccount = require("./service-account-key.json");


  admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
    databaseURL: 'https://epic-system-e245d.firebaseio.com'
  });

  const pushAppNotification = async (fcmToken = [], title, body, refId="", image="") => {
    // for(var i = 0; i < fcmToken.length; i++) {
    // if(fcmToken.length > 0) {
    var message = {
      notification: {
        title,
        body
      },
      tokens: [fcmToken[fcmToken.length - 1]],
      data: {
        id: refId.toString()
      },
      apns: {
        headers: {
          'apns-priority': '10',
        },
        payload: {
          aps: {
            sound: 'default',
            badge: 0
          }
        },
      },
      android: {
        priority: 'high',
        notification: {
          sound: 'default',
        }
      },
    }

    // if(image && image != "") {
    //   message.notification.image = config.baseAPI.concat(image)
    //   message.android.notification.image = config.baseAPI.concat(image)
    //   message.aps = {}
    //   message.aps.fcm_options =  {
    //     image: config.baseAPI.concat(image)
    //   }
    //   message.webpush = {
    //     "headers":{
    //       image: config.baseAPI.concat(image)
    //     }
    //   }
    // }
    console.log("APP_MESSAGE: ", message)
    let result = await admin.messaging().sendMulticast(message)
    if(result.successCount > 0) {
      return true;
    }
    console.log("FCM NOTIF: ", result.responses.map(res => {
      return res.error.code
    }))
    return false;
    // }
    // return false;
  };

  pushAppNotification(['dizsRZvQ9kXmgrYAfoSMw6:APA91bHAwSxOw9LbXohur48VDJsy3ErJ22KyYRCK9J4laE2P1zg80JCeABCd7ySwWdSRcc1DKkyxDMzxugd_pjRdhuUc69x6dkiCBbPgjY-rm9YePEffWU1OnmFUJ-d7annU71yS_Rg1'], 'XIn chào', "Hesllo he'slyly", '1111')
