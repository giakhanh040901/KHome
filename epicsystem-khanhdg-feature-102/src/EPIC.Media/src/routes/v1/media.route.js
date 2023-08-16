const express = require('express');
const validate = require('../../middlewares/validate');
const auth = require('../../middlewares/auth');
const noAuth = require('../../middlewares/noauth');
const uploadFiles = require('../../middlewares/uploadFiles');
const mediaValidation = require('../../validations/media.validation');
const mediaController = require('../../controllers/media.controller');

const router = express.Router();

router
  .route('/')
  .post(auth('provider', 'superadmin'), validate(mediaValidation.createMedia), mediaController.createMedia)
  .get(auth('provider', 'superadmin', 'mobileapp'), validate(mediaValidation.getMedia), mediaController.getMediaList);

router
  .route('/mobile')
  .get(noAuth(), validate(mediaValidation.getMedia), mediaController.getMediaListMobile);


router
  .route('/upload-files')
  .post(auth('provider', 'superadmin'), uploadFiles.array('files', 12), mediaController.uploadImages)

router
  .route('/mobile/:mediaId')
  .get(validate(mediaValidation.getMediaDetail), mediaController.getMediaDetailMobile)

router
  .route('/:mediaId')
  .get(auth('provider', 'superadmin', 'mobileapp'), validate(mediaValidation.getMediaDetail), mediaController.getMediaDetail)
  .patch(auth('provider', 'superadmin'), validate(mediaValidation.updateMedia), mediaController.updateMedia)
  .delete(auth('provider', 'superadmin'), validate(mediaValidation.deleteMedia), mediaController.deleteMedia);

module.exports = router;
