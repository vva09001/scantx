import { all, takeLatest, put } from "redux-saga/effects";
import {
  getScanData,
  editScanData,
  deleteScanData,
  deleteMultiScanData
} from "services/scanData";
import actions from "./actions";

export function* getScanDataSagas(data) {
  const { success, fail } = data;
  try {
    const res = yield getScanData();
    if (res.status === 200) {
      yield success();
      yield put({ type: actions.GET_SCAN_DATA_SUCCESS, response: res.data });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Không thể kết nối đến Sever");
  }
}

export function* editScanDataSagas(data) {
  const { params, success, fail } = data;
  try {
    const res = yield editScanData(params);
    if (res.status === 200) {
      const res = yield getScanData();
      if (res.status === 200) {
        yield success();
        yield put({ type: actions.GET_SCAN_DATA_SUCCESS, response: res.data });
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

export function* deleteScanDataSagas(data) {
  const { id, success, fail } = data;
  try {
    const res = yield deleteScanData(id);
    if (res.status === 200) {
      const res = yield getScanData();
      if (res.status === 200) {
        yield success();
        yield put({ type: actions.GET_SCAN_DATA_SUCCESS, response: res.data });
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

export function* deleteMultiScanDataSagas(data) {
  const { params, success, fail } = data;
  try {
    const res = yield deleteMultiScanData(params);
    if (res.status === 200) {
      const res = yield getScanData();
      if (res.status === 200) {
        yield success();
        yield put({ type: actions.GET_SCAN_DATA_SUCCESS, response: res.data });
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
    yield takeLatest(actions.GET_SCAN_DATA_REQUEST, getScanDataSagas),
    yield takeLatest(actions.EDIT_SCAN_DATA_REQUEST, editScanDataSagas),
    yield takeLatest(actions.DELETE_SCAN_DATA_REQUEST, deleteScanDataSagas),
    yield takeLatest(
      actions.DELETE_MULTI_SCAN_DATA_REQUEST,
      deleteMultiScanDataSagas
    )
  ]);
}
