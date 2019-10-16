import actions from "./actions";
import _ from "lodash";

const initState = {
  list: [],
  qr: "",
  download: "",
  listData: []
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
      };
    case actions.SEARCH_DATA_SUCCESS:
      return {
        ...state,
        listData: _.filter(action.response, item => {
          if (action.key !== "") {
            return item.stationName === action.key;
          } else {
            return item;
          }
        })
      };
    case actions.GET_LIST_DATA_SUCCESS:
      return {
        ...state,
        listData: action.response
      };
    default:
      return state;
  }
}
