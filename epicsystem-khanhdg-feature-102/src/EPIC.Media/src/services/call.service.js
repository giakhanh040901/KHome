const Call = require('../models/call.model');

/**
 * Save a call info
 * @param {ObjectId} caller
 * @param {ObjectId} receiver
 * @param {Moment} startTime
 * @param {Moment} endTime
 * @param {String} duration
 * @param {String} urlAudio
 * @returns {Promise<Call>}
 */
const saveCallInfo = async (callbody) => {
    const { caller, receiver, startTime, endTime,status,duration, urlAudio} = callbody;
    
    const callDoc = await Call.create({
      caller,
      receiver,
      status,
      startTime,
      endTime,
      duration,
      urlAudio
    });
    return callDoc;
  };

/**
 * Query for calls
 * @param {Object} filter - Mongo filter
 * @param {Object} options - Query options
 * @param {string} [options.sortBy] - Sort option in the format: sortField:(desc|asc)
 * @param {number} [options.limit] - Maximum number of results per page (default = 10)
 * @param {number} [options.page] - Current page (default = 1)
 * @returns {Promise<QueryResult>}
 */
const queryCalls = async (filter, options) => {
  const calls = await Call.paginate(filter, options);
  return calls;
};

  module.exports = {
    saveCallInfo,
    queryCalls
  };
