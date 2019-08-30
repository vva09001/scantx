import request from 'helpers/request';

const getUserIdByID = params => {
  return request({
    url: '/start/get-user-by-id',
    method: 'get',
    params: params
  });
};

const getDataScanByUserid = params => {
  return request({
    url: '/start/get-data-scan-by-userid',
    methos: 'get',
    params: params
  })
}

const addUser = params => {
  return request({
    url: '/start/add-user',
    methos: 'get',
    params: params
  })
}

const getCompany = params => {
  return request({
    url: '/company/get-company',
    methos: 'get',
    params: params
  })
}

const editCompany = params => {
  return request({
    url: '/company/edit-company/',
    methos: 'put',
    params: params
  })
}

export { getUserIdByID, getDataScanByUserid, addUser, getCompany, editCompany };
