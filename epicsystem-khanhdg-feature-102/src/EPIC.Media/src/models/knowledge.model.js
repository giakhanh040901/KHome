const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');
const { newsStatus } = require('../config/newsEnums');

const knowledgeSchema = mongoose.Schema(
  {
    title: {
      type: String,
      required: true
    },
    category: {
      type: String,
      enum: [ 'FINANCE', 'TRENDING', 'INVESTMENT', 'CAM_NANG', 'BAO_MAT', 'QUY_TRINH_XU_LY' ],
      required: true,
      default: 'INVESTMENT'
    },
    content: String,
    displayText: String,
    contentType: {
      type: String,
      enum: [ 'HTML', 'MARKDOWN' ],
      default: 'HTML'
    },
    mainImg: String,
    isFeatured: Boolean,
    status: {
      type: String,
      enum: Object.values(newsStatus),
      required: true,
      default: newsStatus.DRAFT
    },
    order: Number,
    trading_provider_id: Number,
    isRoot: {
      type: Boolean,
      default: false
    },
    createdBy: String,
    approveBy: String,
    approveAt: Date,
  },
  {
    timestamps: true,
  });

// add plugin that converts mongoose to json
knowledgeSchema.plugin(toJSON);
knowledgeSchema.plugin(paginate);

const Knowledge = mongoose.model('Knowledge', knowledgeSchema);

module.exports = Knowledge;
