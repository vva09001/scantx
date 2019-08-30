const actions = {
  GET_USER_REQUEST: 'GET_USER_REQUEST',
  GET_USER_SUCCESS: 'GET_USER_SUCCESS',
  getUser: (params, success, fail) => ({
    type: actions.GET_USER_REQUEST,
    params,
    success,
    fail
  }),

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
