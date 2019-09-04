import actions from "./actions";

const initState = { 
  idToken: null,
  profile: {}
};

export default function authReducer(state = initState, action) {
  switch (action.type) {
    case actions.LOGIN_SUCCESS:
      return {
        idToken: action.payload.token,
        profile: action.payload.profile
      };
    case actions.LOGOUT:
      return initState;
    default:
      return state;
  }
}
