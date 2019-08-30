const actions = {
  GET_DATA_REQUEST: 'GET_DATA_REQUEST',
  GET_DATA_SUCCESS: 'GET_DATA_SUCCESS',
  getData: (params, success, fail) => ({
    type: actions.GET_DATA_REQUEST,
    params,
    success,
    fail
  }),
};
export default actions;
