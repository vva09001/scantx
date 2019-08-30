import request from 'helpers/request';

const getScanData = () => {
  return request({
    url: '/scandata/get-scandata',
    method: 'get'
  })
}

export { getScanData };
