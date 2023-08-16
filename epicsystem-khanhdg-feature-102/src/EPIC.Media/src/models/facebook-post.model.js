const mongoose = require('mongoose');
const validator = require('validator');
const bcrypt = require('bcryptjs');
const { toJSON, paginate } = require('./plugins');
const { newsStatus } = require('../config/newsEnums');
const { newsType } = require('../config/newsEnums');


const postSchema = mongoose.Schema(
  {
    pageName: String,
    pageId: String,
    pageImage: String,
    id: String,
    message: String,
    full_picture: String,
    created_time: String,
    updated_time: String,
    permalink_url: String,
    isFeatured: Boolean,
    status: {
      type: String,
      enum: Object.values(newsStatus),
      required: true,
      default: newsStatus.DRAFT
    },
    order: Number,
    sort: {
      type: Number,
      default: 0
    },
    trading_provider_id: Number,
    isRoot: {
      type: Boolean,
      default: false
    },
    postCategory: {
      type: String,
      default: 'main',
      enum: ['main', 'real_estate']
    },
    projectId: Number,
  },
  {
    timestamps: true,
  });

// add plugin that converts mongoose to json
postSchema.plugin(toJSON);
postSchema.plugin(paginate);

const FacebookPost = mongoose.model('FacebookPost', postSchema);

module.exports = FacebookPost;
