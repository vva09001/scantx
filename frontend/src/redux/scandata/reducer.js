import actions from "./actions";
import _ from 'lodash';

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
          if(item.scanId === action.response.scanId) {
            item = action.response;
          }
          return item;
        })
      };
    case actions.DELETE_SCAN_DATA_SUCCESS:
      return {
        ...state, datas: action.response
      };
    case actions.DELETE_MULTI_SCAN_DATA_SUCCESS:
      return {
        ...state, datas: action.response
      };
    default:
      return state;
  }
}
