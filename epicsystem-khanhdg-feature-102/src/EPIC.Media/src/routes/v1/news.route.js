const express = require('express');
const auth = require('../../middlewares/auth');
const validate = require('../../middlewares/validate');
const newsValidation = require('../../validations/news.validation');
const newsController = require('../../controllers/news.controller');
const noAuth = require("../../middlewares/noauth");

const router = express.Router();

router
  .route('/')
  .post(auth('provider', 'superadmin'), validate(newsValidation.createNews), newsController.createNews)
  .get(auth('provider', 'superadmin'), validate(newsValidation.getNews), newsController.getNewsList);

router
  .route('/mobile')
  .get(noAuth(), validate(newsValidation.getNews), newsController.getNewsListMobile);

router
  .route('/mobile/:newsId')
  .get(validate(newsValidation.getNewsDetail), newsController.getNewsDetailMobile)

router
  .route('/:newsId')
  .get(validate(newsValidation.getNewsDetail), newsController.getNewsDetail)
  .patch(auth('provider', 'superadmin'), validate(newsValidation.updateNews), newsController.updateNews)
  .delete(auth('provider', 'superadmin'), validate(newsValidation.deleteNews), newsController.deleteNews);

module.exports = router;
