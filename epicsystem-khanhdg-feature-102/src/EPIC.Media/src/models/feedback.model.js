const mongoose = require('mongoose');
const validator = require('validator');
const bcrypt = require('bcryptjs');
const { toJSON, paginate } = require('./plugins');
const { newsStatus } = require('../config/newsEnums');
const { newsType } = require('../config/newsEnums');


const feedbackSchema = mongoose.Schema({
  title: String,
  detail: String,
  senderName: String,
  senderId: Number,
  status: {
    type: String,
    enum: ['PENDING', 'REPLIED', 'CLOSE'],
    required: true,
    default: 'PENDING'
  },
  attachments: [{
    name: String,
    url: String
  }],
  category: {
    type: String,
    enum: ['GOP_Y'],
    required: true,
    default: ''
  },
  reply: [{
    senderName: String,
    senderId: Number,
    createdAt: { type: Date, default: Date.now},
    content: String
  }]
  },
  {
    timestamps: true,
  });

// add plugin that converts mongoose to json
feedbackSchema.plugin(toJSON);
feedbackSchema.plugin(paginate);

const Feedback = mongoose.model('Feedback', feedbackSchema);

module.exports = Feedback;
