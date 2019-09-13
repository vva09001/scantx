import { all, takeEvery, put } from 'redux-saga/effects';
import { push } from 'react-router-redux';
import { clearToken, getToken } from '../../helpers/utility';
import { login, register } from 'services/auth';
import actions from './actions';

export function* loginRequest(data) {
  const { params, success, fail } = data;
  try {
    const res = yield login(params);
    if (res.status === 200) {
      yield put({
        type: actions.LOGIN_SUCCESS,
        payload: { 
          token: res.data.token,
          profile: res.data
        }
      });
      yield success();
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail('Cannot connect to Server');
  }
}

export function* loginSuccess({ payload }) {
  yield localStorage.setItem('id_token', payload.token);
}

export function* loginError() {}

export function* logout() {
  clearToken();
  yield put(push('/'));
}
export function* checkAuthorization() {
  const token = getToken();
  if (token) {
    yield put({
      type: actions.LOGIN_SUCCESS,
      payload: { token },
      profile: 'Profile',
    });
  }
}

export function* registerSaga(data) {
  const { params, success, fail } = data;
  try {
    const res = yield register(params);
    if (res.status === 200) {
      yield success();
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail('Cannot connect to Server');
  }
}

export default function* rootSaga() {
  yield all([
    yield takeEvery(actions.CHECK_AUTHORIZATION, checkAuthorization),
    yield takeEvery(actions.LOGIN_REQUEST, loginRequest),
    yield takeEvery(actions.LOGIN_SUCCESS, loginSuccess),
    yield takeEvery(actions.LOGIN_ERROR, loginError),
    yield takeEvery(actions.LOGOUT, logout),
    yield takeEvery(actions.REGISTER_REQUEST, registerSaga),
  ]);
}
