const nodemailer = require('nodemailer');
const config = require('../config/config');
const logger = require('../config/logger');
var markdown = require('nodemailer-markdown').markdown;

const HTMLDecoderEncoder = require("html-encoder-decoder");
const { text } = require("express");



function getParameterByName(name, url) {
  name = name.replace(/[\[\]]/g, '\\$&');
  var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
    results = regex.exec(url);
  if (!results) return null;
  if (!results[2]) return '';
  return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

/* istanbul ignore next */
if (config.env !== 'test') {
  let transport = nodemailer.createTransport(config.email.smtp);
  transport
    .verify()
    .then(() => logger.info('Connected to email server'))
    .catch(() => logger.warn('Unable to connect to email server. Make sure you have configured the SMTP options in .env'));
}

/**
 * Send an email
 * @param {string} to
 * @param {string} subject
 * @param {string} text
 * @returns {Promise}
 */
const sendEmail = async (notificationConfiguration, to, subject, text, data={}, type="HTML", attachments=[]) => {
  const transport = nodemailer.createTransport({
    host: notificationConfiguration.stmpConfig.SMTP_HOST,
    port: notificationConfiguration.stmpConfig.SMTP_PORT,
    auth: {
      user: notificationConfiguration.stmpConfig.SMTP_USERNAME,
      pass: notificationConfiguration.stmpConfig.SMTP_PASSWORD,
    },
  });
  transport.use('compile', markdown());
  const msg = { from: {
      name: notificationConfiguration.stmpConfig.EMAIL_BRAND_NAME,
      address: notificationConfiguration.stmpConfig.EMAIL_FROM
    }, to, subject };
  if(type == 'HTML') {
    msg.html = replaceContent(HTMLDecoderEncoder.decode(text), data);
  }else if(type == 'MARKDOWN') {
    msg.markdown = replaceContent(text, data);
  }else {
    msg.text = replaceContent(text, data);
  }
  if(attachments != null) {
    msg.attachments = attachments
      .filter(attatchment => attatchment != null)
      .map(attatchment => {
        return  {
          filename: getParameterByName("file", attatchment),
          path: attatchment
        }
      })
  }
  return transport.sendMail(msg);
};

const replaceContent = function(text, data){
  Object.keys(data).forEach(keyToReplace => {
    text = text.replace("[".concat(keyToReplace).concat("]"), data[keyToReplace])
  })
  return text
}
/**
 * Send reset password email
 * @param {string} to
 * @param {string} token
 * @returns {Promise}
 */
const sendResetPasswordEmail = async (to, token) => {
  const subject = 'Reset password';
  // replace this url with the link to the reset password page of your front-end app
  const resetPasswordUrl = `http://link-to-app/reset-password?token=${token}`;
  const text = `Dear user,
To reset your password, click on this link: ${resetPasswordUrl}
If you did not request any password resets, then ignore this email.`;
  await sendEmail(to, subject, text);
};

/**
 * Send verification email
 * @param {string} to
 * @param {string} token
 * @returns {Promise}
 */
const sendVerificationEmail = async (to, token) => {
  const subject = 'Email Verification';
  // replace this url with the link to the email verification page of your front-end app
  const verificationEmailUrl = `http://link-to-app/verify-email?token=${token}`;
  const text = `Dear user,
To verify your email, click on this link: ${verificationEmailUrl}
If you did not create an account, then ignore this email.`;
  await sendEmail(to, subject, text);
};

module.exports = {
  sendEmail,
  sendResetPasswordEmail,
  sendVerificationEmail,
};
