const express = require('express');
const router = express.Router();
const callController = require("../../controllers/call.controller");
const validate = require('../../middlewares/validate');
const auth = require('../../middlewares/auth');
const callValidation = require('../../validations/call.validation');
const noAuth = require('../../middlewares/noauth');

router
  .route('/')
  .post(auth('mobileapp'),validate(callValidation.createCallInfo),callController.createCall)
  .get(auth('provider', 'superadmin'),validate(callValidation.getHistoryCall),callController.getCallList);

  module.exports = router;
