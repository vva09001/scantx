import { all, takeLatest, put } from 'redux-saga/effects';
import { getDataScanByUserid } from 'services/data';
import actions from './actions';

export function* getDataSagas(data) {
  const { params, success, fail } = data;
  try {
    const res = yield getDataScanByUserid(params);
    if (res.status === 200) {
      yield success();
      yield put({type: actions.GET_DATA_SUCCESS, response: res.data.detail});
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail('Không thể kết nối đến Sever');
  }
}

export default function* rootSaga() {
  yield all([
    yield takeLatest(actions.GET_DATA_REQUEST, getDataSagas)
  ]);
}
