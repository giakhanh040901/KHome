const express = require('express');
const auth = require('../../middlewares/auth');
const validate = require('../../middlewares/validate');
const feedbackValidation = require('../../validations/feedback.validation');
const feedbackController = require('../../controllers/feedback.controller');
const noAuth = require("../../middlewares/noauth");

const router = express.Router();

router
  .route('/')
  .post(auth('mobileapp'), validate(feedbackValidation.createFeedback), feedbackController.createFeedback)
  .get(auth('provider', 'superadmin'), validate(feedbackValidation.getFeedback), feedbackController.getFeedbackList);

router
  .route('/mobile')
  .get(auth('mobileapp'), validate(feedbackValidation.getFeedback), feedbackController.getFeedbackListMobile);

router
  .route('/mobile/:feedbackId')
  .get( validate(feedbackValidation.getFeedbackDetail), feedbackController.getFeedbackDetailMobile);

router
  .route('/:feedbackId')
  .get( validate(feedbackValidation.getFeedbackDetail), feedbackController.getFeedbackDetail)
  .patch(auth('provider', 'superadmin'), validate(feedbackValidation.updateFeedback), feedbackController.updateFeedback)
  .delete(auth('provider', 'superadmin'), validate(feedbackValidation.deleteFeedback), feedbackController.deleteFeedback);

module.exports = router;
