const passport = require('passport');
const httpStatus = require('http-status');
const ApiError = require('../utils/ApiError');
const { roleRights, userType } = require('../config/roles');
const jwkToPem = require("jwk-to-pem");
const jwt = require("jsonwebtoken");
const express = require("express");
const jwtHelper = require("../helpers/jwk")


const auth = (...requiredRights) => async (req, res, next) => {
  if(!req.header('authorization')) {
    return res.status(401).json( "Lỗi không có token");
  }
  const token = req.header('authorization').replace('Bearer ', "");
  const jwk = jwtHelper.readJWK();
  const pem = jwkToPem(jwk);

  try {
    const payload = jwt.verify(token, pem);
    if(payload.user_type == 'T' || payload.user_type == userType.ROOT_TRADING_PROVIDER) {
      if(requiredRights.indexOf("provider") == -1) {
        return res.status(403).json("Bạn không có quyền truy cập API này");
      }
      payload.isSuperAdmin = false;
    }else if(payload.user_type == 'S' || payload.user_type == 'P' || payload.user_type == 'E') {
      if((requiredRights.indexOf("superadmin") == -1)) {
        return res.status(403).json("Bạn không có quyền truy cập API này");
      }
      payload.isSuperAdmin = true;
    }else if(payload.user_type == 'I') {
      if((requiredRights.indexOf("mobileapp") == -1)) {
        return res.status(403).json("Bạn không có quyền truy cập API này");
      }
      payload.isSuperAdmin = false;
      payload.isTrader = true;
    } else if(payload.user_type == userType.ROOT_EPIC) {
      payload.isSuperAdmin = true;
    }else if(payload.user_type == 'RP') {
      payload.isSuperAdmin = false;
      payload.isTrader = false;
    }else {
      return res.status(401).json("Forbidden");
    }
    var tradingProvidersCanAccess = null
    if(payload.user_type == userType.INVESTOR) {
      if(!payload.investor_trading_id_defaults) {
        tradingProvidersCanAccess = null
      } else if (payload.investor_trading_id_defaults.length == 0) {
        tradingProvidersCanAccess = null
      } else if(Array.isArray(payload.investor_trading_id_defaults)) {
        tradingProvidersCanAccess = { $in: payload.investor_trading_id_defaults }
      } else {
        tradingProvidersCanAccess = payload.investor_trading_id_defaults
      }
    }else  if(payload.user_type == userType.PARTNER) {
      if(payload.trading_provider_ids) {
        tradingProvidersCanAccess = { $in: payload.trading_provider_ids }
      } else {
        tradingProvidersCanAccess = { $in: [] }
      }

    }else if(payload.user_type == userType.TRADING_PROVIDER || payload.user_type == userType.ROOT_TRADING_PROVIDER) {
      tradingProvidersCanAccess = { $in: [payload.trading_provider_id] }
      // tradingProvidersCanAccess = null
    } else if(payload.user_type == userType.ROOT || payload.user_type == userType.ROOT_EPIC) {
      tradingProvidersCanAccess = null
    }
    console.log("tradingProvidersCanAccess", tradingProvidersCanAccess)
    req.tradingProvidersCanAccess = tradingProvidersCanAccess
    // req.tradingProvidersCanAccess = null
    req.user = payload;
  } catch (err) {
    return res.status(401).json(err);
  }
  next();
};

module.exports = auth;
