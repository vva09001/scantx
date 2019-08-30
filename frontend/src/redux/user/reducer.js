import actions from "./actions";

const initState = { 
  info: {} ,
  scanData: {}
};

export default function userReducer(state = initState, action) {
  switch (action.type) {
    case actions.GET_USER_SUCCESS:
      return {
        info: action.response
      };
    default:
      return state;
  }
}
