const express = require('express');
const auth = require('../../middlewares/auth');
const validate = require('../../middlewares/validate');
const notificationValidation = require('../../validations/notification.validation');
const notificationController = require('../../controllers/notification.controller');
const notificationConfigController = require('../../controllers/notification-config.controller');

const router = express.Router();

router
  .route('/')
  .post(auth('provider', 'superadmin'), validate(notificationValidation.createNotification), notificationController.createNotification)
  .get(auth('provider', 'superadmin', 'mobileapp'), validate(notificationValidation.getNotification), notificationController.getNotificationList);

router
  .route('/system')
  .post(auth('provider', 'superadmin'), validate(notificationValidation.createOrUpdateSystemNotificationValidation), notificationController.createOrUpdateNotificationTemplate)
  .get(auth('provider', 'superadmin', 'mobileapp'), validate(notificationValidation.getNotification), notificationController.getSystemNotification);

router
  .route('/send')
  .post(auth('provider', 'superadmin'), validate(notificationValidation.sendNotification), notificationController.sendNotification);

router
  .route('/send-system/:trading_provider_id')
  .post(validate(notificationValidation.createSystemNotification), notificationController.sendSystemNotification);

router
  .route('/delete-customer')
  .delete(auth('provider', 'superadmin'), validate(notificationValidation.deleteSendingList), notificationController.deletePersonInList)

router
  .route('/get-read/:sentNotificationId')
  .get(auth('mobileapp'), validate(notificationValidation.setReadValidation), notificationController.getReadMobile)

router
  .route('/list-mobile')
  .get(auth('mobileapp'), notificationController.getSentNotificationMobileList)
  // .get(notificationController.getSentNotificationMobileList)

router
  .route('/delete-notification')
  .delete(auth('mobileapp'), notificationController.deleteNotificationMobileList)

router
  .route('/read-all-notification/')
  .patch(auth('mobileapp'), notificationController.readAllNotificationMobileList)

router
  .route('/delete-single-notification/:id')
  .delete(auth('mobileapp'), notificationController.deleteSingleNotificationMobile)


router
  .route("/sending-list/:notificationId")
  .get(auth('provider', 'superadmin'), validate(notificationValidation.getSendingList), notificationController.getSendingList)
  .post(auth('provider', 'superadmin'), validate(notificationValidation.addPersonToSendingList), notificationController.updatePersonList)

router
  .route('/config')
  .put(auth('provider'), validate(notificationValidation.saveNotificationConfig), notificationConfigController.saveNotificationConfiguration)
  .get(auth('provider'), notificationConfigController.getNotificationConfiguration);


router
  .route('/:notificationId')
  .get(auth('provider', 'superadmin', 'mobileapp'), validate(notificationValidation.getNotificationDetail), notificationController.getNotificationDetail)
  .patch(auth('provider', 'superadmin'), validate(notificationValidation.updateNotification), notificationController.updateNotification)
  .delete(auth('provider', 'superadmin'), validate(notificationValidation.deleteNotification), notificationController.deleteNotification);


module.exports = router;
