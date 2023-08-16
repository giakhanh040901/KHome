var bunyan = require('bunyan');

var logFileLocation = './logs';

var logger = bunyan.createLogger({
  name: 'epic-node-api',
  streams: [{
    type: 'rotating-file',
    level: 'info',
    path: logFileLocation + '/epic-info.log',
    period: '1d',   // daily rotation
    count: 10        // keep 3 back copies
  }, {
    type: 'rotating-file',
    level: 'debug',
    path: logFileLocation + '/epic-debug.log',
    period: '1d',   // daily rotation
    count: 10        // keep 3 back copies
  }, {
    type: 'rotating-file',
    level: 'error',
    path: logFileLocation + '/epic-error.log',
    period: '1d',   // daily rotation
    count: 10        // keep 3 back copies
  }]
});

module.exports = logger;
