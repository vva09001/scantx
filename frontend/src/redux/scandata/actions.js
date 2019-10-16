const actions = {
  GET_SCAN_DATA_REQUEST: "GET_SCAN_DATA_REQUEST",
  GET_SCAN_DATA_SUCCESS: "GET_SCAN_DATA_SUCCESS",
  getScanData: (success, fail) => ({
    type: actions.GET_SCAN_DATA_REQUEST,
    success,
    fail
  }),
  EDIT_SCAN_DATA_REQUEST: "EDIT_SCAN_DATA_REQUEST",
  EDIT_SCAN_DATA_SUCCESS: "EDIT_SCAN_DATA_SUCCESS",
  edit: (params, success, fail) => ({
    type: actions.EDIT_SCAN_DATA_REQUEST,
    params,
    success,
    fail
  }),

  ADD_SCAN_DATA_REQUEST: "ADD_SCAN_DATA_REQUEST",
  ADD_SCAN_DATA_SUCCESS: "ADD_SCAN_DATA_SUCCESS",
  add: (params, success, fail) => ({
    type: actions.ADD_SCAN_DATA_REQUEST,
    params,
    success,
    fail
  }),

  DELETE_SCAN_DATA_REQUEST: "DELETE_SCAN_DATA_REQUEST",
  DELETE_SCAN_DATA_SUCCESS: "DELETE_SCAN_DATA_SUCCESS",
  delete: (id, success, fail) => ({
    type: actions.DELETE_SCAN_DATA_REQUEST,
    id,
    success,
    fail
  }),

  DELETE_MULTI_SCAN_DATA_REQUEST: "DELETE_MULTI_SCAN_DATA_REQUEST",
  DELETE_MULTI_SCAN_DATA_SUCCESS: "DELETE_MULTI_SCAN_DATA_SUCCESS",
  deleteMulti: (params, success, fail) => ({
    type: actions.DELETE_MULTI_SCAN_DATA_REQUEST,
    params,
    success,
    fail
  }),

  GET_QR_REQUEST: "GET_QR_REQUEST",
  GET_QR_SUCCESS: "GET_QR_SUCCESS",
  getQr: (success, fail) => ({
    type: actions.GET_QR_REQUEST,
    success,
    fail
  }),

  DOWNLOAD_SCAN_DATA_REQUEST: "DOWNLOAD_SCAN_DATA_REQUEST",
  DOWNLOAD_SCAN_DATA_SUCCESS: "DOWNLOAD_SCAN_DATA_SUCCESS",
  download: (id, success, fail) => ({
    type: actions.DOWNLOAD_SCAN_DATA_REQUEST,
    id,
    success,
    fail
  }),

  SEARCH_DATA_REQUEST: "SEARCH_DATA_REQUEST",
  SEARCH_DATA_SUCCESS: "SEARCH_DATA_SUCCESS",
  search: key => ({
    type: actions.SEARCH_DATA_REQUEST,
    key
  }),

  GET_LIST_DATA_REQUEST: "GET_LIST_DATA_REQUEST",
  GET_LIST_DATA_SUCCESS: "GET_LIST_DATA_SUCCESS",
  getListData: (success, fail) => ({
    type: actions.GET_LIST_DATA_REQUEST,
    success,
    fail
  }),
};
export default actions;
