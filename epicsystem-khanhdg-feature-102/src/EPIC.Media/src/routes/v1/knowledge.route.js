const express = require('express');
const auth = require('../../middlewares/auth');
const validate = require('../../middlewares/validate');
const knowledgeValidation = require('../../validations/knowledge.validation');
const knowledgeController = require('../../controllers/knowledge.controller');
const noAuth = require("../../middlewares/noauth");

const router = express.Router();

router
  .route('/')
  .post(auth('provider', 'superadmin'), validate(knowledgeValidation.createKnowledge), knowledgeController.createKnowledge)
  .get(auth('provider', 'superadmin'), validate(knowledgeValidation.getKnowledge), knowledgeController.getKnowledgeList);

router
  .route('/mobile')
  .get(noAuth(), validate(knowledgeValidation.getKnowledge), knowledgeController.getKnowledgeListMobile);

router
  .route('/mobile/:knowledgeId')
  .get( validate(knowledgeValidation.getKnowledgeDetail), knowledgeController.getKnowledgeDetailMobile);

router
  .route('/:knowledgeId')
  .get( validate(knowledgeValidation.getKnowledgeDetail), knowledgeController.getKnowledgeDetail)
  .patch(auth('provider', 'superadmin'), validate(knowledgeValidation.updateKnowledge), knowledgeController.updateKnowledge)
  .delete(auth('provider', 'superadmin'), validate(knowledgeValidation.deleteKnowledge), knowledgeController.deleteKnowledge);

module.exports = router;
