import actions from "./actions";
import _ from "lodash";

const initState = {
  list: []
};

export default function userReducer(state = initState, action) {
  switch (action.type) {
    case actions.GET_USER_SUCCESS:
      return {
        list: action.response
      };
    case actions.EDIT_USER_SUCCESS:
      return {
        list: _.map(state.list, item => {
          if (item.id === action.response.id) {
            item = action.response;
          }
          return item;
        })
      };
    case actions.ADD_USER_SUCCESS:
      return {
        list: [...state.list, action.response]
      };
    case actions.DELETE_USER_SUCCESS:
      return {
        list: _.filter(state.list, item => {
          return item.id !== action.response.id;
        })
      };
    case actions.DELETE_MULTI_USER_SUCCESS:
      return {
        list: _.filter(state.list, item => {
          return !_.includes(action.response, item.id);
        })
      };
    default:
      return state;
  }
}
