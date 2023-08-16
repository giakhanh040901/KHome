const jwt = require("jsonwebtoken");
const { roleRights, userType } = require('../config/roles');
const {getGarnerDescription, createGarnerDescription, getGarnerDescriptionByFilter} = require("../services/garner-description.service");
const {newsStatus, contentType} = require("../config/newsEnums");

module.exports.checkDescriptionExist = () => async (req, res, next) => {
  //req.user.trading_provider_id
  let filter = {}
  if(req.user.user_type == 'E' || req.user.user_type == 'RE') {
    filter.isRoot = true
  }else {
    filter.isRoot = false
    filter.trading_provider_id =  req.user.trading_provider_id
  }
  let garnerDesc = await getGarnerDescriptionByFilter(filter)
  //Nếu chưa có thì tạo mới'

  if(!garnerDesc) {
    garnerDesc = await createGarnerDescription({
      tradingProviderName: "",
      images: [],
      description: "",
      features: [],
      trading_provider_id: req.user.trading_provider_id,
      isRoot: filter.isRoot
    })
  }
  req.garnerDesc = garnerDesc
  next()
}
