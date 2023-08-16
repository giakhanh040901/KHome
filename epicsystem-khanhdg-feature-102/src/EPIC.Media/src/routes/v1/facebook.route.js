const express = require('express');
const validate = require('../../middlewares/validate');
const auth = require('../../middlewares/auth');
const facebookValidation = require('../../validations/facebook.validation');
const facebookController = require('../../controllers/facebook.controller');
const noAuth = require("../../middlewares/noauth");

const router = express.Router();

router
  .route('/exchange-token')
  .post(validate(facebookValidation.exchangeTokenValidation), facebookController.exchangeLonglivedToken)

router
  .route('/add-post')
  .post(auth('provider', 'superadmin'), validate(facebookValidation.addFacebookPostValidation), facebookController.addFacebookPost)

router
  .route('/update-post/:postId')
  .post(auth('provider', 'superadmin'), validate(facebookValidation.updatePost), facebookController.updatePost)

router
  .route('/update-status/:postId')
  .post(auth('provider', 'superadmin'), validate(facebookValidation.updatePostStatus), facebookController.updatePostStatus)


router
  .route('/list-post-ids')
  .get(auth('provider', 'superadmin'), facebookController.checkPostsExist)

router
  .route('/list-post')
  .get(auth('provider', 'superadmin'), facebookController.getPostList)
router
  .route('/list-post-mobile')
  .get(noAuth(), facebookController.getPostListMobile)

module.exports = router;
