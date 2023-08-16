const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');
const { newsStatus, contentType } = require('../config/newsEnums');
const { newsType } = require('../config/newsEnums');

const productImageSchema = mongoose.Schema(
  {
    code: String,
    title: {
      type: String,
      required: true
    },
    type: {
      type: String,
      enum: ['bond', 'estate_invest', 'invest_product'],
      required: true
    },
    displayText: String,
    content: String,
    sort: Number,
    contentType: {
      type: String,
      enum: Object.values(contentType),
      default: contentType.MARKDOWN
    },
    productId: String,
    isFeatured: Boolean,
    isRoot: Boolean,
    mainImg: String,
    status: {
      type: String,
      enum: Object.values(newsStatus),
      required: true,
      default: newsStatus.DRAFT
    },
    trading_provider_id: Number,
    user_type: String,
    createdBy: String
  },
  {
    timestamps: true,
  });

// add plugin that converts mongoose to json
productImageSchema.plugin(toJSON);
productImageSchema.plugin(paginate);

const ProductImage = mongoose.model('ProductImage', productImageSchema);

module.exports = ProductImage;
