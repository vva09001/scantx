const actions = {
  GET_SCAN_DATA_REQUEST: 'GET_SCAN_DATA_REQUEST',
  GET_SCAN_DATA_SUCCESS: 'GET_SCAN_DATA_SUCCESS',
  getScanData: (success, fail) => ({
    type: actions.GET_SCAN_DATA_REQUEST,
    success,
    fail
  }),
};
export default actions;
