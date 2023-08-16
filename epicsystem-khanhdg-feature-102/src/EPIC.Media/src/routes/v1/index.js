const express = require('express');
const userRoute = require('./user.route');
const newsRoute = require('./news.route');
const mediaRoute = require('./media.route');
const productImageRoute = require('./product-image.route');
const notificationRoute = require('./notification.route');
const knowledgeBase = require('./knowledge.route');
const garnerDescriptionRoute = require('./garnerDescription.route');
const notificationTemplateRoute = require('./notification-template.route');
const feedbackRoute = require('./feedback.route');
const facebookRoute = require('./facebook.route');
const callRoute = require('./call.route');
const config = require('../../config/config');
const auth = require('../../middlewares/auth');
const path = require("path");
const { genQRcode } = require("../../controllers/qr-code.controller");
const {configFolder} = require("../../config/configFolder");

const router = express.Router();

const defaultRoutes = [
  {
    path: '/user',
    route: userRoute,
  },{
    path: '/news',
    route: newsRoute,
  },
  {
    path: '/media',
    route: mediaRoute,
  },
  {
    path: '/notification-template',
    route: notificationTemplateRoute,
  },
  {
    path: '/notification',
    route: notificationRoute,
  },
  {
    path: '/knowledge-base',
    route: knowledgeBase,
  },
  {
    path: '/product-image',
    route: productImageRoute,
  },
  {
    path: '/feedback',
    route: feedbackRoute,
  },
  {
    path: '/garner',
    route: garnerDescriptionRoute,
  },
  {
    path: '/facebook',
    route: facebookRoute,
  },
  {
    path: '/call',
    route: callRoute,
  },
];

router.get('/me', auth('superadmin', 'provider'), (req, res) => {
  res.send(req.user)
})

router.get('/rocket_chat_auth_get',(req, res) => {
  res.send({ loginToken: 'YmVteN9Fv1lj3-fc8Yz6A49vn8617-ovPtCQCDGbpEQ' })
})

router.get('/health', (req, res) => {
  res.send({ status: "OK" })
})

router.get('/qr-code', genQRcode)
defaultRoutes.forEach((route) => {
  router.use(route.path, route.route);
});

router.get("/image/:fileName", (req, res) => {
  res.sendFile(path.join(configFolder.PATH_UPLOAD + req.params.fileName));
});

module.exports = router;
