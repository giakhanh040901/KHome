const mongoose = require('mongoose');
const validator = require('validator');
const bcrypt = require('bcryptjs');
const { toJSON, paginate } = require('./plugins');
const { newsStatus, newsType,navigationTypes,levelOneNavigationOptions } = require('../config/newsEnums');

const newsSchema = mongoose.Schema(
  {
    code: String,
    title: {
      type: String,
      required: true
    },
    type: {
      type: String,
      enum: Object.values(newsType),
      required: true,
      default: newsType.PURE_NEWS
    },
    displayText: String,
    contentType: {
      type: String,
      enum: [ 'HTML', 'MARKDOWN' ],
      default: 'HTML'
    },
    content: String,
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
    user_type: String,
    isRoot: {
      type: Boolean,
      default: false
    },
    createdBy: String,
    approveBy: String,
    approveAt: Date,
    isNavigation: {
      type: Boolean,
      default: false
    },
    navigationType: {
      type: String,
      enum: Object.values(navigationTypes),
      default: navigationTypes.NULL,
    },
    levelOneNavigation: {
      type: String,
      enum: Object.values(levelOneNavigationOptions),
      default: levelOneNavigationOptions.NULL,
    },
    secondLevelNavigation: {
      type: String,
      required: false
    },
    navigationLink: String,
    typeModule: {
      type: String,
      required: false
    }
  },
  {
    timestamps: true,
  });

// add plugin that converts mongoose to json
newsSchema.plugin(toJSON);
newsSchema.plugin(paginate);

const News = mongoose.model('News', newsSchema);

module.exports = News;
