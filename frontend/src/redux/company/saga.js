import { all, takeLatest, put, select } from "redux-saga/effects";
import { get, add, edit, remove } from "services/company";
import { getToken } from "redux/selectors";
import actions from "./actions";

export function* getCompanySagas(data) {
  const { success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield get(token);
    if (res.status === 200) {
      yield success();
      yield put({ type: actions.GET_COMPANY_SUCCESS, response: res.data });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* addCompanySagas(data) {
  const { params, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield add(params, token);
    if (res.status === 200) {
      yield success();
      yield put({ type: actions.ADD_COMPANY_SUCCESS, response: res.data.data });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* editCompanySagas(data) {
  const { params, success, fail } = data;
  const body = {
    address: params.address,
    cid: params.cid,
    name: params.name,
    status: params.status
  };

  try {
    const token = yield select(getToken);
    const res = yield edit(body, token);
    if (res.status === 200) {
      yield put({
        type: actions.EDIT_COMPANY_SUCCESS,
        response: res.data.data
      });
      yield success();
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* deleteCompanySagas(data) {
  const { params, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield remove(params, token);
    if (res.status === 200) {
      yield put({ type: actions.DELETE_COMPANY_SUCCESS, response: params });
      yield success();
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export default function* rootSaga() {
  yield all([
    yield takeLatest(actions.GET_COMPANY_REQUEST, getCompanySagas),
    yield takeLatest(actions.ADD_COMPANY_REQUEST, addCompanySagas),
    yield takeLatest(actions.EDIT_COMPANY_REQUEST, editCompanySagas),
    yield takeLatest(actions.DELETE_COMPANY_REQUEST, deleteCompanySagas)
  ]);
}
