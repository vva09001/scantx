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
        datas: action.response
      };
    case actions.DELETE_COMPANY_SUCCESS:
      return {
        datas: _.filter(state.list, item => {
          console.log(!_.includes(action.response, item.cid))
          if(!_.includes(action.response, item.cid)) {
            return item;
          }
        })
      };
    default:
      return state;
  }
}
