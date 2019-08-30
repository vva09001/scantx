import request from 'helpers/request';

const getDataScanByUserid = params => {
  return request({
    url: '/start/get-data-scan-by-userid',
    method: 'get',
    params: params
  })
}

export { getDataScanByUserid };
