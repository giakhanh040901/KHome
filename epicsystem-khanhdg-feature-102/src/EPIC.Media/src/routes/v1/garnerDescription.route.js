const express = require('express');
const auth = require('../../middlewares/auth');
const { checkDescriptionExist } = require('../../middlewares/garnerDescription');
const validate = require('../../middlewares/validate');
const garnerValidation = require('../../validations/garner-description.validation');
const garnerDescController = require('../../controllers/garner-description.controller');
const noAuth = require("../../middlewares/noauth");
const mediaValidation = require("../../validations/media.validation");
const mediaController = require("../../controllers/media.controller");

const router = express.Router();

router
  .route('/')
  .post(auth('provider', 'superadmin'), checkDescriptionExist(), validate(garnerValidation.updateGarnerProductDescription), garnerDescController.updateGarnerDesc)
  .get(auth('provider', 'superadmin'), checkDescriptionExist(), garnerDescController.getGarnerDesc);

router
  .route('/images')
  .post(auth('provider', 'superadmin'), checkDescriptionExist(), validate(garnerValidation.updateGarnerProductImages), garnerDescController.updateGarnerDescImages)

router
  .route('/features')
  .post(auth('provider', 'superadmin'), checkDescriptionExist(), validate(garnerValidation.updateGarnerProductFeatures), garnerDescController.updateGarnerDescFeatures)


router
  .route('/mobile')
  .get(noAuth(), garnerDescController.getGarnerDescMobile);


module.exports = router;
