import actions from "./actions";
import _ from "lodash";

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
        list: _.map(state.list, item => {
          if (item.scanId === action.response.scanId) {
            item = action.response;
          }
          return item;
        })
      };
    case actions.ADD_SCAN_DATA_SUCCESS:
      return {
        list: [action.response, ...state.list]
      };
    case actions.DELETE_SCAN_DATA_SUCCESS:
      return {
        list: _.filter(state.list, item => {
          return item.scanId !== action.response.scanId;
        })
      };
    case actions.DELETE_MULTI_SCAN_DATA_SUCCESS:
      return {
        list: _.filter(state.list, item => {
          return !_.includes(action.response, item.scanId);
        })
      };
    default:
      return state;
  }
}
