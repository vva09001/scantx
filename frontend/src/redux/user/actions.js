const actions = {
  GET_USER_REQUEST: 'GET_USER_REQUEST',
  GET_USER_SUCCESS: 'GET_USER_SUCCESS',
  getUser: (success, fail) => ({
    type: actions.GET_USER_REQUEST,
    success,
    fail
  }),

  ADD_USER_REQUEST: 'ADD_USER_REQUEST',
  ADD_USER_SUCCESS: 'ADD_USER_SUCCESS',
  addUser: (params, success, fail) => ({
    type: actions.ADD_USER_REQUEST,
    params,
    success,
    fail
  }),
};
export default actions;
