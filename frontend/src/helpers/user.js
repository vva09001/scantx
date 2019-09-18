const permission = {
  user: {
    getAll: [1],
    add: [1, 2],
    addAdmin: [1]
  },
  company: {
    add: [1],
    delete: [1]
  },
  scanData: {
    add: [1, 2, 3]
  }
};

const role = roleID => {
  if (roleID === 1) {
    return "superadmin";
  } else if (roleID === 2) {
    return "admin";
  } else if (roleID === 3) {
    return "user";
  } else if (roleID === 4) {
    return "reader";
  } else {
    return "No Role";
  }
};

const generatePassword = () => {
  const length = 8;
  const charset = "23456789abcdefghmnpqrstuvwxyzABCDEFGHLMNPQRSTUVWXYZ";
  const n = charset.length;
  let password = "";
  for (let i = 0; i < length; ++i) {
    password += charset.charAt(Math.floor(Math.random() * n));
  }
  return password;
};

export { permission, role, generatePassword };
