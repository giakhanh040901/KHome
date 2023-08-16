const express = require('express');
const auth = require('../../middlewares/auth');
const validate = require('../../middlewares/validate');
const notificationTemplateValidation = require('../../validations/notification-template.validation');
const notificationTemplateController = require('../../controllers/notification-template.controller');

const router = express.Router();

router
  .route('/')
  .post(auth('provider', 'superadmin'), validate(notificationTemplateValidation.createNotificationTemplate), notificationTemplateController.createNotificationTemplate)
  .get(auth('provider', 'superadmin', 'mobileapp'), validate(notificationTemplateValidation.getNotificationTemplate), notificationTemplateController.getNotificationTemplateList);


router
  .route('/:notificationTemplateId')
  .get(auth('provider', 'superadmin', 'mobileapp'), validate(notificationTemplateValidation.getNotificationTemplateDetail), notificationTemplateController.getNotificationTemplateDetail)
  .patch(auth('provider', 'superadmin'), validate(notificationTemplateValidation.updateNotificationTemplate), notificationTemplateController.updateNotificationTemplate)
  .delete(auth('provider', 'superadmin'), validate(notificationTemplateValidation.deleteNotificationTemplate), notificationTemplateController.deleteNotificationTemplate);

module.exports = router;
