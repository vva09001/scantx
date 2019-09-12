import request from "helpers/request";

const getUsers = (id, token) => {
  return request({
    url: "/user/get-user-by-id",
    method: "get",
    params: {
      id: id
    },
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

const addUser = (params, token) => {
  return request({
    url: "/user/register",
    method: "post",
    data: params,
    headers: {
      Authorization: `Bearer ${token}`
    }
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

export { getUsers, addUser, editUser, deleteUserById, deleteMultiUser };
