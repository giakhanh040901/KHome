const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');
const { newsStatus, contentType } = require('../config/newsEnums');
const { newsType,navigationTypes,
    levelOneNavigationOptions, imageType } = require('../config/newsEnums');

const mediaSchema = mongoose.Schema(
  {
    code: String,
    title: {
      type: String,
      required: true
    },
    type: {
      type: String,
      enum: [
        'uu_dai_cua_toi',
        'kham_pha',
        'dau_tu',
        'tai_san',
        'tich_luy',
        'san_pham_dau_tu',
        'san_pham',
        'mua_bds',
        'thue_bds',
        'videos',
        'vi_sao_su_dung_epic'
      ],
      required: true,
      default: 'kham_pha'
    },
    displayText: {
      type: String,
      required: false
    },
    content: String,
    description: String,
    sort: Number,
    contentType: {
      type: String,
      enum: Object.values(contentType),
      default: contentType.MARKDOWN
    },
    mainImg: String,
    imageType: {
      type: String,
      enum: Object.values(imageType),
      default: imageType.IMAGE
    },
    position: String,
    productKey: String,
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
    },
  },
  {
    timestamps: true,
  });

// add plugin that converts mongoose to json
mediaSchema.plugin(toJSON);
mediaSchema.plugin(paginate);

const Media = mongoose.model('Media', mediaSchema);

module.exports = Media;
