const actions = {
  GET_COMPANY_REQUEST: 'GET_COMPANY_REQUEST',
  GET_COMPANY_SUCCESS: 'GET_COMPANY_SUCCESS',
  getCompany: (success, fail) => ({
    type: actions.GET_COMPANY_REQUEST,
    success,
    fail
  }),

  EDIT_COMPANY_REQUEST: 'EDIT_COMPANY_REQUEST',
  EDIT_COMPANY_SUCCESS: 'EDIT_COMPANY_SUCCESS',
  editCompany: (params, success, fail) => ({
    type: actions.EDIT_COMPANY_REQUEST,
    params,
    success,
    fail
  }),

  DELETE_COMPANY_REQUEST: 'DELETE_COMPANY_REQUEST',
  DELETE_COMPANY_SUCCESS: 'DELETE_COMPANY_SUCCESS',
  editCompany: (id, success, fail) => ({
    type: actions.DELETE_COMPANY_REQUEST,
    id,
    success,
    fail
  }),
};
export default actions;
