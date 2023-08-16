const allRoles = {
  user: [],
  admin: ['getUsers', 'manageUsers'],
};

const roles = Object.keys(allRoles);
const roleRights = new Map(Object.entries(allRoles));

const userType = {
  TRADING_PROVIDER: 'T',
  ROOT_TRADING_PROVIDER: 'RT',
  PARTNER: 'P',
  ROOT: 'E',
  ROOT_EPIC: 'RE',
  INVESTOR: 'I'
}
module.exports = {
  roles,
  roleRights,
  userType
};
