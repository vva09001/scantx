const actions = {
  GET_DATA_REQUEST: 'GET_DATA_REQUEST',
  GET_DATA_SUCCESS: 'GET_DATA_SUCCESS',
  getData: (success, fail) => ({
    type: actions.GET_DATA_REQUEST,
    success,
    fail
  }),
};
export default actions;
