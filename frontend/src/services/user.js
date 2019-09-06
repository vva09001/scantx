import request from 'helpers/request';

const getUsers = token => {
    return request({
      url: '/user/get-all-user',
      method: 'get',
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  };

const addUser = (params, token) => {
    return request({
      url: '/user/register',
      method: 'post',
      data: params,
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
  }

export { getUsers, addUser };
