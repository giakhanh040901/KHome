const catchAsync = require("../utils/catchAsync");
const pick = require("../utils/pick");
const { userService } = require("../services");

const getMe = catchAsync(async (req, res) => {
  const filter = pick(req.query, ['name', 'role']);
  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  const result = await userService.queryUsers(filter, options);
  res.send(result);
});

module.exports = {
  getMe
};
