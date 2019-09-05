import request from "helpers/request";

const getScanData = token => {
  return request({
    url: "/scandata/get-scandata",
    method: "get",
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

const editScanData = (params, token) => {
  return request({
    url: "/scandata/edit-scandata/",
    method: "put",
    data: params,
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

const addScanData = (params, token) => {
  return request({
    url: "/scandata/add-scandata/",
    method: "post",
    data: params,
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

const deleteScanData = (id, token) => {
  return request({
    url: "/scandata/delete-scandata/",
    method: "post",
    params: {
      scanid: id
    },
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

const deleteMultiScanData = (params, token) => {
  return request({
    url: "/scandata/delete-arr-scandata/",
    method: "post",
    data: params,
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
};

export {
  getScanData,
  editScanData,
  addScanData,
  deleteScanData,
  deleteMultiScanData
};
