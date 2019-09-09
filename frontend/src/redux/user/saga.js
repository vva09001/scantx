import { all, takeLatest, put, select } from "redux-saga/effects";
import {
  getUsers,
  addUser,
  editUser,
  deleteUserById,
  deleteMultiUser
} from "services/user";
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
      yield success();
      yield put({
        type: actions.ADD_USER_SUCCESS,
        response: res.data.data
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Không thể kết nối đến Sever");
  }
}

export function* editUserSagas(data) {
  const { params, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield editUser(params, token);
    if (res.status === 200) {
      yield success();
      yield put({
        type: actions.EDIT_USER_SUCCESS,
        response: res.data.data
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Không thể kết nối đến Sever");
  }
}

export function* deleteUserSagas(data) {
  const { id, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield deleteUserById(id, token);
    if (res.status === 200) {
      yield success();
      yield put({
        type: actions.DELETE_USER_SUCCESS,
        response: res.data.data
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Không thể kết nối đến Sever");
  }
}

export function* deleteMultiUserSagas(data) {
  const { params, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield deleteMultiUser(params, token);
    if (res.status === 200) {
      yield success();
      yield put({
        type: actions.DELETE_MULTI_USER_SUCCESS,
        response: res.data.data
      });
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
    yield takeLatest(actions.ADD_USER_REQUEST, addUserSagas),
    yield takeLatest(actions.EDIT_USER_REQUEST, editUserSagas),
    yield takeLatest(actions.DELETE_USER_REQUEST, deleteUserSagas),
    yield takeLatest(actions.DELETE_MULTI_USER_REQUEST, deleteMultiUserSagas)
  ]);
}
