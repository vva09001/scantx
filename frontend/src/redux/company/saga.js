import { all, takeLatest, put } from 'redux-saga/effects';
import { get, add, edit, remove } from 'services/company';
import actions from './actions';

export function* getCompnaySagas(data) {
  const { success, fail } = data;
  try {
    const res = yield get();
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

export function* addCompnaySagas(data) {
  const { params, success, fail } = data;
  try {
    const res = yield add(params);
    if (res.status === 200) {
      yield success();
      yield put({type: actions.ADD_COMPANY_SUCCESS, response: res.data});
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
    const res = yield edit(params);
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
  const { params, success, fail } = data;
  console.log(params)
  // try {
  //   const res = yield remove(params);
  //   if (res.status === 200) {
  //     yield put({type: actions.DELETE_COMPANY_SUCCESS, response: params});
  //     yield success();
  //   } else {
  //     yield fail(res.data.message);
  //   }
  // } catch (error) {
  //   yield fail('Không thể kết nối đến Sever');
  // }
}

export default function* rootSaga() {
  yield all([
    yield takeLatest(actions.GET_COMPANY_REQUEST, getCompnaySagas),
    yield takeLatest(actions.ADD_COMPANY_REQUEST, addCompnaySagas),
    yield takeLatest(actions.EDIT_COMPANY_REQUEST, editCompnaySagas),
    yield takeLatest(actions.DELETE_COMPANY_REQUEST, deleteCompnaySagas)
  ]);
}
