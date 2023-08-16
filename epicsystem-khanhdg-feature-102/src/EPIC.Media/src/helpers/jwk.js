const fs = require('fs');
const jwk = require('../../signing-key.json')
module.exports = {
  readJWK: readJWK
};
function toLowerKeys(obj) {
  const entries = Object.entries(obj);
  return Object.fromEntries(
    entries.map(([key, value]) => {
      return [key.toLowerCase(), value];
    }),
  );
}

function readJWK() {
  return toLowerKeys(jwk)
}
