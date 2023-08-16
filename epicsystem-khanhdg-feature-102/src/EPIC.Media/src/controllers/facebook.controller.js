const httpStatus = require('http-status');
const pick = require('../utils/pick');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
var request = require('request');
const {feedbackService, facebookService, newsService, mediaService} = require("../services");
const {newsStatus} = require("../config/newsEnums");
const fs = require('fs');
const axios = require('axios');
const uuid = require('uuid');
const {configFolder} = require("../config/configFolder");

const FACEBOOK_TOKEN_EXCHANGE_API = 'https://graph.facebook.com/v3.2/oauth/access_token?grant_type=fb_exchange_token' +
  '&client_id={app-id}' +
  '&client_secret={app-secret}' +
  '&fb_exchange_token={short-lived-token}';
const FACEBOOK_APP_SECRET = '22a968cdadae2828d33c69f2afa066d7'
const FACEBOOK_APP_ID = '288859401588883'

const exchangeLonglivedToken = catchAsync(async (req, res) => {
  var shortLivedUserToken = req.body.token;

  if (!shortLivedUserToken) {
    return res.status(400).json('bad request');
  }

  var exchangeTokenUrl = FACEBOOK_TOKEN_EXCHANGE_API.replace('{app-id}', FACEBOOK_APP_ID)
    .replace('{app-secret}', FACEBOOK_APP_SECRET)
    .replace('{short-lived-token}', shortLivedUserToken);

  request({
    url: exchangeTokenUrl,
    json: true
  }, function (error, response, body) {
    if (!error && response.statusCode === 200) {
      return res.status(200).json(body);
    }
    return res.status(500).json(response);
  });
});

const addFacebookPost = catchAsync(async (req, res) => {
  req.body.trading_provider_id = req.user.trading_provider_id;
  if(req.user.user_type == 'E' || req.user.user_type == 'RE') {
    req.body.isRoot = true
  }
  let postBody = req.body;
  if(!postBody.id) {
    postBody.id = Date.now()
  }
  if(!postBody.postCategory) {
    postBody.postCategory = 'main'
  }
  const checkExist = await facebookService.getFacebookPostByPostId(postBody.id, req.body.trading_provider_id)
  if(checkExist) {
    return res.status(httpStatus.LOCKED).send({message: 'Bài viết đã tồn tại'});
  }
  if(postBody.full_picture && !postBody.full_picture.includes('api/')) {
    console.log("full_picture______",postBody.full_picture);
    const imageExt = 'full-image-' + uuid.v4() + '.jpg';
    console.log("imageExt______",imageExt);
    const imagePath = configFolder.PATH_UPLOAD.concat(imageExt);
    console.log("imagePath______",imagePath);
    await download_image(postBody.full_picture, imagePath);
    postBody.full_picture = 'api/media/image/' + imageExt
    console.log("postBody.full_picture______",postBody.full_picture);
  }
  if(postBody.pageImage && !postBody.pageImage.includes('api/')) {
    const pageImagePathExt = 'page-image-' + uuid.v4() + '.jpg';
    const pageImagePath = configFolder.PATH_UPLOAD.concat(pageImagePathExt);
    await download_image(postBody.pageImage, pageImagePath);
    postBody.pageImage = 'api/media/image/' + pageImagePathExt
  }

  const post = await facebookService.createFacebookPost(postBody);
  res.status(httpStatus.CREATED).send(post);
})

const checkPostsExist = catchAsync(async (req, res) => {
  let isRoot = false;
  if(req.user.user_type == 'E' || req.user.user_type == 'RE') {
     isRoot = true
  }

  let filter = {
    isRoot
  }
  if(req.user.trading_provider_id) {
    filter.trading_provider_id = req.user.trading_provider_id
  }
  let allPosts = await facebookService.queryFacebookPost(filter)
  let postIds = allPosts.map(post => {
    return post.id
  })
  return res.status(httpStatus.OK).json(postIds);
})


const getPostList = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['q', 'status', 'postCategory', 'projectId']);

  if(filter.q) {
    filter.$or = [{
      message: { "$regex": filter.q, "$options": "i" }
    }, {
      id: { "$regex": filter.q, "$options": "i" }
    }]
    delete filter.q;
  }



  if(req.tradingProvidersCanAccess) {
    filter.trading_provider_id = req.tradingProvidersCanAccess
  }else {
    filter.isRoot = true;
  }

  if(!filter.postCategory) {
    filter.postCategory = 'main'
  }else {
    delete filter.isRoot
    delete filter.trading_provider_id
  }


  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "updated_time:desc";
  }

  const result = await facebookService.queryFacebookPostAll(filter, options);
  res.send(result);

});

const getPostListMobile = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['q', 'status', 'postCategory', 'projectId']);
  // Lọc danh sách dữ liệu theo trading provider.
  /**
   * Comment ngày 29, sau bỏ đi
   */

  if(req.tradingProvidersCanAccess) {
    filter.trading_provider_id = req.tradingProvidersCanAccess
  }else {
    filter.isRoot = true;
  }

  if(!filter.postCategory) {
    filter.postCategory = 'main'
  }else {
    delete filter.isRoot
    delete filter.trading_provider_id
  }
  /**
   * Kết thúc comment
   */
  if(filter.q) {
    filter.$or = [ {
      message: { "$regex": filter.q, "$options": "i" }
    }, {
      id: { "$regex": filter.q, "$options": "i" }
    }]
    delete filter.q;
  }

  filter.status = newsStatus.ACTIVE
  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  if(!options.sortBy) {
    options.sortBy = "updated_time:desc";
  }
  const result = await facebookService.queryFacebookPostAll(filter, options);
  res.send({
    status: 1,
    data: result,
    code: 200,
    message: 'OK'
  });
});



const updatePost = catchAsync(async (req, res) => {
  if(!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  const post = await facebookService.updateFacebookPostById(req.params.postId, req.body);
  res.send(post);
});

const updatePostStatus = catchAsync(async (req, res) => {
  if(!req.user.isSuperAdmin) {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  const post = await facebookService.updateFacebookPostStatusById(req.params.postId, req.body);
  res.send(post);
});


const download_image = (url, image_path) =>
  axios({
    url,
    responseType: 'stream',
  }).then(
    response =>
      new Promise((resolve, reject) => {
        response.data
          .pipe(fs.createWriteStream(image_path))
          .on('finish', () => resolve())
          .on('error', e => reject(e));
      }),
  );

module.exports = {
  exchangeLonglivedToken,
  addFacebookPost,
  checkPostsExist,
  getPostList,
  getPostListMobile,
  updatePost,
  updatePostStatus
};

