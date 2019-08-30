import actions from "./actions";

const initState = { 
  list: [],
};

export default function userReducer(state = initState, action) {
  switch (action.type) {
    case actions.GET_USER_SUCCESS:
      return {
        list: action.response
      };
    default:
      return state;
  }
}
