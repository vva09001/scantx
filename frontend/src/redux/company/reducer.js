import actions from "./actions";

const initState = {
  companies: []
};

export default function companyReducer(state = initState, action) {
  switch (action.type) {
    case actions.GET_COMPANY_SUCCESS:
      return {
        datas: action.response
      };
    case actions.EDIT_COMPANY_SUCCESS:
      return {
        datas: action.response
      };
    case actions.DELETE_COMPANY_SUCCESS:
      return {
        datas: action.response
      };
    default:
      return state;
  }
}
