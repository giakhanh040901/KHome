const express = require('express');
const helmet = require('helmet');
const xss = require('xss-clean');
const mongoSanitize = require('express-mongo-sanitize');
const compression = require('compression');
const cors = require('cors');
const passport = require('passport');
const httpStatus = require('http-status');
const config = require('./config/config');
const morgan = require('./config/morgan');
const { jwtStrategy } = require('./config/passport');
const { authLimiter } = require('./middlewares/rateLimiter');
const routes = require('./routes/v1');
const { errorConverter, errorHandler } = require('./middlewares/error');
const ApiError = require('./utils/ApiError');
const jwk = require('./helpers/jwk');
var session = require('express-session')
const { createBullBoard } = require('@bull-board/api');
const { BullAdapter } = require('@bull-board/api/bullAdapter');
const { BullMQAdapter } = require('@bull-board/api/bullMQAdapter');
const { ExpressAdapter } = require('@bull-board/express');
const Queue = require("bull");


const app = express();

if (config.env !== 'test') {
  app.use(morgan.successHandler);
  app.use(morgan.errorHandler);
}


app.set('jwk', jwk.readJWK());
// set security HTTP headers
app.use(helmet());

// parse json request body
app.use(express.json({limit: '50mb'}));

// parse urlencoded request body
app.use(express.urlencoded({ extended: true, limit: '50mb' }));

// sanitize request data
app.use(xss());
app.use(mongoSanitize());

// gzip compression
app.use(compression());

// // enable cors
// const corsOptions = {
//   origin: 'http://epic-core.stecom.vn:3000',
//   credentials: true,
//
// }
// app.use(cors(corsOptions));
//
app.use('*', cors());

// jwt authentication
app.use(passport.initialize());
passport.use('jwt', jwtStrategy);


// limit repeated failed requests to auth endpoints
if (config.env === 'production') {
  app.use('/v1/auth', authLimiter);
}

// limit repeated failed requests to auth endpoints
if (config.env === 'production') {
  // v1 api routes
  app.use('/v1', routes);
  // app.use('/api/media', routes);
}else if(config.env === 'development') {
  // v1 api routes
  app.use('/api/media', routes);
}


const serverAdapter = new ExpressAdapter();
serverAdapter.setBasePath('/api/media/admin/queues');



const notificationQueue = new Queue('notification-queue', config.redisUrl);
const pushNotificationQueue = new Queue('push-notification-queue', config.redisUrl);

const { addQueue, removeQueue, setQueues, replaceQueues } = createBullBoard({
  queues: [
    new BullAdapter(pushNotificationQueue),
    new BullAdapter(notificationQueue)
  ],
  serverAdapter: serverAdapter,
});
app.use('/v1/admin/queues', serverAdapter.getRouter());


// send back a 404 error for any unknown api request
app.use((req, res, next) => {
  next(new ApiError(httpStatus.NOT_FOUND, 'Not found'));
});

// convert error to ApiError, if needed
app.use(errorConverter);

// handle error
app.use(errorHandler);


module.exports = app;
