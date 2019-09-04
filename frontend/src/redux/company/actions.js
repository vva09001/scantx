const actions = {
  GET_COMPANY_REQUEST: 'GET_COMPANY_REQUEST',
  GET_COMPANY_SUCCESS: 'GET_COMPANY_SUCCESS',
  get: (success, fail) => ({
    type: actions.GET_COMPANY_REQUEST,
    success,
    fail
  }),

  ADD_COMPANY_REQUEST: 'ADD_COMPANY_REQUEST',
  ADD_COMPANY_SUCCESS: 'ADD_COMPANY_SUCCESS',
  add: (params, success, fail) => ({
    type: actions.ADD_COMPANY_REQUEST,
    params,
    success,
    fail
  }),

  EDIT_COMPANY_REQUEST: 'EDIT_COMPANY_REQUEST',
  EDIT_COMPANY_SUCCESS: 'EDIT_COMPANY_SUCCESS',
  edit: (params, success, fail) => ({
    type: actions.EDIT_COMPANY_REQUEST,
    params,
    success,
    fail
  }),

  DELETE_COMPANY_REQUEST: 'DELETE_COMPANY_REQUEST',
  DELETE_COMPANY_SUCCESS: 'DELETE_COMPANY_SUCCESS',
  delete: (params, success, fail) => ({
    type: actions.DELETE_COMPANY_REQUEST,
    params,
    success,
    fail
  }),
};
export default actions;
