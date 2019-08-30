import actions from "./actions";

const initState = {
  datas: {}
};

export default function dataReducer(state = initState, action) {
  switch (action.type) {
    case actions.GET_DATA_SUCCESS:
      return {
        datas: action.response
      };
    default:
      return state;
  }
}
