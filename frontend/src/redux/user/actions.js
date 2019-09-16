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

  EDIT_USER_REQUEST: 'EDIT_USER_REQUEST',
  EDIT_USER_SUCCESS: 'EDIT_USER_SUCCESS',
  editUser: (params, success, fail) => ({
    type: actions.EDIT_USER_REQUEST,
    params,
    success,
    fail
  }),

  DELETE_USER_REQUEST: 'DELETE_USER_REQUEST',
  DELETE_USER_SUCCESS: 'DELETE_USER_SUCCESS',
  deleteUser: (id, success, fail) => ({
    type: actions.DELETE_USER_REQUEST,
    id,
    success,
    fail
  }),

  DELETE_MULTI_USER_REQUEST: 'DELETE_MULTI_USER_REQUEST',
  DELETE_MULTI_USER_SUCCESS: 'DELETE_MULTI_USER_SUCCESS',
  deleteMultiUser: (params, success, fail) => ({
    type: actions.DELETE_MULTI_USER_REQUEST,
    params,
    success,
    fail
  }),
};
export default actions;
