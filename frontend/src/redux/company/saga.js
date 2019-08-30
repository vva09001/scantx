import { all, takeLatest, put } from 'redux-saga/effects';
import { getCompany, editCompany, deleteCompany } from 'services/company';
import actions from './actions';

export function* getCompnaySagas(data) {
  const { success, fail } = data;
  try {
    const res = yield getCompany();
    if (res.status === 200) {
      yield success();
      yield put({type: actions.GET_COMPANY_SUCCESS, response: res.data});
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail('Không thể kết nối đến Sever');
  }
}

export function* editCompnaySagas(data) {
  const { params, success, fail } = data;
  try {
    const res = yield editCompany(params);
    if (res.status === 200) {
      yield success();
      yield put({type: actions.EDIT_COMPANY_SUCCESS, response: res.data});
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail('Không thể kết nối đến Sever');
  }
}

export function* deleteCompnaySagas(data) {
  const { id, success, fail } = data;
  try {
    const res = yield deleteCompany(id);
    if (res.status === 200) {
      yield success();
      yield put({type: actions.DELETE_COMPANY_SUCCESS, response: res.data});
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail('Không thể kết nối đến Sever');
  }
}

export default function* rootSaga() {
  yield all([
    yield takeLatest(actions.GET_COMPANY_REQUEST, getCompnaySagas),
    yield takeLatest(actions.EDIT_COMPANY_REQUEST, editCompnaySagas),
    yield takeLatest(actions.DELETE_COMPANY_REQUEST, deleteCompnaySagas)
  ]);
}
