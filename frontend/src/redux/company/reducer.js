import actions from "./actions";
import _ from 'lodash';

const initState = {
  list: []
};

export default function companyReducer(state = initState, action) {
  switch (action.type) {
    case actions.GET_COMPANY_SUCCESS:
      return {
        list: action.response
      };
    case actions.ADD_COMPANY_SUCCESS:
      return {
        list: [action.response, ...state.list]
      };
    case actions.EDIT_COMPANY_SUCCESS:
      return {
        list: _.map(state.list, item => {
          if(item.cid === action.response.cid) {
            item = action.response;
          }
          return item;
        })
      };
    case actions.DELETE_COMPANY_SUCCESS:
      return {
        list: _.filter(state.list, item => {
          return !_.includes(action.response, item.cid)
        })
      };
    default:
      return state;
  }
}
