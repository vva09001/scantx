const actions = {
  CHECK_AUTHORIZATION: 'CHECK_AUTHORIZATION',
  LOGIN_REQUEST: 'LOGIN_REQUEST',
  LOGOUT: 'LOGOUT',
  LOGIN_SUCCESS: 'LOGIN_SUCCESS',
  LOGIN_PROFILE_SUCCESS: 'LOGIN_PROFILE_SUCCESS',
  LOGIN_ERROR: 'LOGIN_ERROR',
  checkAuthorization: () => ({ type: actions.CHECK_AUTHORIZATION }),
  login: (params, success, fail) => ({
    type: actions.LOGIN_REQUEST,
    params,
    success,
    fail
  }),
  logout: () => ({
    type: actions.LOGOUT
  }),

  REGISTER_REQUEST: 'REGISTER_REQUEST',
  register: (params, success, fail) => ({
    type: actions.REGISTER_REQUEST,
    params,
    success,
    fail
  })
};
export default actions;
