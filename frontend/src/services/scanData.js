import request from 'helpers/request';

const getScanData = () => {
  return request({
    url: '/scandata/get-scandata',
    method: 'get'
  })
}

const editScanData = params => {
  return request({
      url: '/scandata/edit-scandata/',
      method: 'put',
      data: params
  })
}

const deleteScanData = id => {
  return request({
      url: '/scandata/delete-scandata/',
      method: 'post',
      params: {
          scanid: id
      }
  })
}

const deleteMultiScanData = params => {
  return request({
      url: '/scandata/delete-arr-scandata/',
      method: 'post',
      data: params
  })
}

export { getScanData, editScanData, deleteScanData, deleteMultiScanData };
