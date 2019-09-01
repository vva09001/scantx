import actions from "./actions";

const initState = {
  list: []
};

export default function scanDataReducer(state = initState, action) {
  switch (action.type) {
    case actions.GET_SCAN_DATA_SUCCESS:
      return {
        list: action.response
      };
    case actions.EDIT_SCAN_DATA_SUCCESS:
      return {
        datas: action.response
      };
    case actions.DELETE_SCAN_DATA_SUCCESS:
      return {
        datas: action.response
      };
    default:
      return state;
  }
}
