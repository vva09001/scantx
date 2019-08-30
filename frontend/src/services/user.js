import request from 'helpers/request';

const getUserIdByID = params => {
    return request({
      url: '/start/get-user-by-id',
      method: 'get',
      params: params
    });
  };

const addUser = params => {
    return request({
      url: '/start/add-user',
      method: 'get',
      params: params
    })
  }

export { getUserIdByID, addUser };
