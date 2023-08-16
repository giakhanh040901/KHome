
const axios = require('axios');

const ONE_SMS_ENDPONIT="https://api.conek.vn/SendSms"
/**
 * Send an sms
 * @param {number} phone
 * @param {string} message
 * @param {number} unicode
 * @returns {Promise}
 */
const sendSMS = async (notificationConfiguration, phone, message, unicode= "0") => {
  let data = {
    "username": notificationConfiguration.smsConfig.USERNAME,
    "password": notificationConfiguration.smsConfig.PASSWORD,
    "brandname": notificationConfiguration.smsConfig.BRANDNAME,
    "phone": phone,
    "message": message,
    "unicode": unicode
  }
  console.log(data)
  return axios.post(ONE_SMS_ENDPONIT, data);
};

module.exports = {
  sendSMS
}

