import { all, takeLatest, put, select } from "redux-saga/effects";
import { getUsers, addUser } from "services/user";
import { getToken } from "redux/selectors";
import actions from "./actions";

export function* getUserSagas(data) {
  const { success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield getUsers(token);
    if (res.status === 200) {
      yield success();
      yield put({ type: actions.GET_USER_SUCCESS, response: res.data });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Không thể kết nối đến Sever");
  }
}

export function* addUserSagas(data) {
  const { params, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield addUser(params, token);
    if (res.status === 200) {
      const res = yield getUsers(token);
      if (res.status === 200) {
        yield success();
        yield put({ type: actions.GET_USER_SUCCESS, response: res.data });
      } else {
        yield fail(res.data.message);
      }
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Không thể kết nối đến Sever");
  }
}

export default function* rootSaga() {
  yield all([
    yield takeLatest(actions.GET_USER_REQUEST, getUserSagas),
    yield takeLatest(actions.ADD_USER_REQUEST, addUserSagas)
  ]);
}
