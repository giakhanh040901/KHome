const jwt = require("jsonwebtoken");
const { roleRights, userType } = require('../config/roles');

const noAuth = () => async (req, res, next) => {
  if(!req.header('authorization')) {
    req.tradingProvidersCanAccess = null;
    next();
  }else {
    const token = req.header('authorization').replace('Bearer ', "");
    try {
      const payload = jwt.decode(token);
      console.log(payload)
      var tradingProvidersCanAccess = null
      if(payload.user_type == userType.INVESTOR) {
        if(!payload.investor_trading_id_defaults) {
          tradingProvidersCanAccess = null
        } else if(Array.isArray(payload.investor_trading_id_defaults)) {
          tradingProvidersCanAccess = { $in: payload.investor_trading_id_defaults }
        } else {
          tradingProvidersCanAccess = payload.investor_trading_id_defaults
        }
        if(Array.isArray(payload.investor_trading_id_defaults) && payload.investor_trading_id_defaults.length >= 2) {
          tradingProvidersCanAccess = null
        }
      }

      if(payload.user_type == userType.ROOT_EPIC || payload.user_type == 'S') {
        payload.isSuperAdmin = true;
      }

      req.tradingProvidersCanAccess = tradingProvidersCanAccess;
      req.user = payload
      console.log(req.tradingProvidersCanAccess)
    } catch (err) {
      return res.status(401).json(err);
    }
    next();
  }
};

module.exports = noAuth;
