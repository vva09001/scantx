import { all, takeLatest, put } from 'redux-saga/effects';
import { getScanData } from 'services/scanData';
import actions from './actions';

export function* getScanDataSagas(data) {
  const { success, fail } = data;
  try {
    const res = yield getScanData();
    if (res.status === 200) {
      yield success();
      yield put({type: actions.GET_SCAN_DATA_SUCCESS, response: res.data});
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail('Không thể kết nối đến Sever');
  }
}

export default function* rootSaga() {
  yield all([
    yield takeLatest(actions.GET_SCAN_DATA_REQUEST, getScanDataSagas)
  ]);
}
