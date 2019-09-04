import request from 'helpers/request';

const login = params => {
    return request({
      url: '/user/authenticate',
      method: 'post',
      params: params
    });
  };

const register = params => {
    return request({
      url: '/user/register',
      method: 'get',
      data: params
    })
  }

export { login, register };
