const express = require('express');
const auth = require('../../middlewares/auth');
const validate = require('../../middlewares/validate');
const uploadFiles = require('../../middlewares/uploadFiles');
const productImageValidation = require('../../validations/product-image.validation');
const productImageController = require('../../controllers/product-image.controller');
const noAuth = require("../../middlewares/noauth");

const router = express.Router();

router
  .route('/')
  .post(noAuth(), validate(productImageValidation.createProductImage), productImageController.createProductImage)
  .get(noAuth(), validate(productImageValidation.getProductImage), productImageController.getProductImageList);

router
  .route('/mobile')
  .get(noAuth(), validate(productImageValidation.getProductImage), productImageController.getProductImageListMobile);

router
  .route('/mobile/:productImageId')
  .get(validate(productImageValidation.getProductImageDetail), productImageController.getProductImageDetailMobile)

router
  .route('/:productImageId')
  .get(auth('provider', 'superadmin', 'mobileapp'), validate(productImageValidation.getProductImageDetail), productImageController.getProductImageDetail)
  .patch(noAuth(), validate(productImageValidation.updateProductImage), productImageController.updateProductImage)
  .delete(auth('provider', 'superadmin'), validate(productImageValidation.deleteProductImage), productImageController.deleteProductImage);

module.exports = router;
