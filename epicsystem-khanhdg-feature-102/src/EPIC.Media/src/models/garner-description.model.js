const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');
const { newsStatus, contentType } = require('../config/newsEnums');
const { types } = require('../config/garnerEnums');

const GarnerDescriptionSchema = mongoose.Schema(
  {
    tradingProviderName: String,
    tradingProviderId: Number,
    images: [{
      id: Number,
      order: Number,
      position: {
        type: String,
        enum: [ 'banner', 'hinh_anh_noi_bat', 'slider'],
        default: 'slider'
      },
      path: String,
      status: {
        type: String,
        enum: Object.values(newsStatus),
        default: newsStatus.PENDING
      },
      event: {
        screen: String,
        params: Object
      },
      order: Number
    }],
    description: String,
    descriptionContentType: {
      type: String,
      enum: Object.values(contentType),
      default: contentType.MARKDOWN
    },
    features: [
      {
        order: Number,
        iconUri: String,
        description: String,
        status: {
          type: String,
          enum: Object.values(newsStatus),
          default: 'pending'
        },
        fileUrl: String,
        type: {
          type: String,
          enum: Object.values(types),
          required: true,
          default: types.FILE
        },
      }
    ],
    status: {
      type: String,
      enum: Object.values(newsStatus),
      required: true,
      default: newsStatus.DRAFT
    },
    trading_provider_id: Number,
    user_type: String,
    isRoot: {
      type: Boolean,
      default: false
    },

  },
  {
    timestamps: true,
  });

GarnerDescriptionSchema.plugin(toJSON);
GarnerDescriptionSchema.plugin(paginate);

const Media = mongoose.model('GarnerDescription', GarnerDescriptionSchema);

module.exports = Media;
