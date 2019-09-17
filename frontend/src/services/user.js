import request from "helpers/request";

const getUsers = token => {
  return request({
    url: "/user/get-user",
    method: "get",
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

const addUser = (params, token) => {
  return request({
    url: "/user/adduser",
    method: "post",
    data: params,
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

const registerUser = params => {
  return request({
    url: "/user/register",
    method: "post",
    data: params
  });
};

const editUser = (params, token) => {
  return request({
    url: "/user/update",
    method: "put",
    data: params,
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

const deleteUserById = (params, token) => {
  return request({
    url: "/user/delete-user",
    method: "post",
    data: params,
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

const deleteMultiUser = (params, token) => {
  return request({
    url: "/user/delete-arr-user",
    method: "post",
    data: params,
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

export {
  getUsers,
  addUser,
  registerUser,
  editUser,
  deleteUserById,
  deleteMultiUser
};
