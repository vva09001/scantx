import actions from "./actions";
import _ from "lodash";

const initState = {
  list: [],
  qr: ""
};

export default function scanDataReducer(state = initState, action) {
  switch (action.type) {
    case actions.GET_SCAN_DATA_SUCCESS:
      return {
        ...state,
        list: action.response
      };
    case actions.EDIT_SCAN_DATA_SUCCESS:
      return {
        ...state,
        list: _.map(state.list, item => {
          if (item.scanId === action.response.scanId) {
            item = action.response;
          }
          return item;
        })
      };
    case actions.ADD_SCAN_DATA_SUCCESS:
      return {
        ...state,
        list: [action.response, ...state.list]
      };
    case actions.DELETE_SCAN_DATA_SUCCESS:
      return {
        ...state,
        list: _.filter(state.list, item => {
          return item.scanId !== action.response.scanId;
        })
      };
    case actions.DELETE_MULTI_SCAN_DATA_SUCCESS:
      return {
        ...state,
        list: _.filter(state.list, item => {
          return !_.includes(action.response, item.scanId);
        })
      };
    case actions.GET_QR_SUCCESS:
      return {
        ...state,
        qr: action.response
      }
    case actions.DOWNLOAD_SCAN_DATA_SUCCESS:
      return state;
    default:
      return state;
  }
}
