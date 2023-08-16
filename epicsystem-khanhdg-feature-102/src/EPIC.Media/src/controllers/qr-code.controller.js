const catchAsync = require("../utils/catchAsync");
const httpStatus = require("http-status");
const QRCode = require('qrcode')

const genQRcode = catchAsync(async (req, res) => {
  const str = await QRCode.toDataURL(req.query.content)
  res.status(httpStatus.CREATED).send({
    content: req.query.content,
    base64: str
  });
});

module.exports = {
  genQRcode
}
