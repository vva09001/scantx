import { all, takeLatest, put, select, takeEvery } from "redux-saga/effects";
import {
  getScanData,
  editScanData,
  addScanData,
  deleteScanData,
  deleteMultiScanData,
  getQr,
  downloadScanData
} from "services/scanData";
import { getToken } from "redux/selectors";
import { Error } from "helpers/notify";
import actions from "./actions";

export function* getScanDataSagas(data) {
  const { success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield getScanData(token);
    if (res.data.status === "200") {
      yield success();
      yield put({
        type: actions.GET_SCAN_DATA_SUCCESS,
        response: res.data.data
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* editScanDataSagas(data) {
  const { params, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield editScanData(params, token);
    if (res.data.status === "200") {
      yield success();
      yield put({
        type: actions.EDIT_SCAN_DATA_SUCCESS,
        response: res.data.data
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* addScanDataSagas(data) {
  const { params, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield addScanData(params, token);
    if (res.data.status === "200") {
      yield success();
      yield put({
        type: actions.ADD_SCAN_DATA_SUCCESS,
        response: res.data.data
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* deleteScanDataSagas(data) {
  const { id, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield deleteScanData(id, token);
    if (res.data.status === "200") {
      yield success();
      yield put({
        type: actions.DELETE_SCAN_DATA_SUCCESS,
        response: res.data.data
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* deleteMultiScanDataSagas(data) {
  const { params, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield deleteMultiScanData(params, token);
    if (res.data.status === "200") {
      yield success();
      yield put({
        type: actions.DELETE_MULTI_SCAN_DATA_SUCCESS,
        response: res.data.data
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* getQrSagas(data) {
  const { success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield getQr(token);
    if (res.data.status === "200") {
      yield success();
      yield put({ type: actions.GET_QR_SUCCESS, response: res.data.data });
    } else {
      yield Error(res.data.message);
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* downloadScanDataSagas(data) {
  const { id, success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield downloadScanData(id, token);

    if (res.data.status === "200") {
      const win = window.open("http://" + res.data.data, "_blank");
      win.focus();
      yield success();
      yield put({
        type: actions.DOWNLOAD_SCAN_DATA_SUCCESS
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export function* searchSaga(data) {
  try {
    const token = yield select(getToken);
    const res = yield getScanData(token);
    if (res.data.status === "200") {
      yield put({
        type: actions.SEARCH_DATA_SUCCESS,
        response: res.data.data,
        key: data.key
      });
    } else {
      yield Error(res.data.message);
    }
  } catch (error) {
    yield Error("Cannot connect to Server");
  }
}

export function* listScanDataSagas(data) {
  const { success, fail } = data;
  try {
    const token = yield select(getToken);
    const res = yield getScanData(token);
    if (res.data.status === "200") {
      yield success();
      yield put({
        type: actions.GET_LIST_DATA_SUCCESS,
        response: res.data.data
      });
    } else {
      yield fail(res.data.message);
    }
  } catch (error) {
    yield fail("Cannot connect to Server");
  }
}

export default function* rootSaga() {
  yield all([
    yield takeLatest(actions.GET_SCAN_DATA_REQUEST, getScanDataSagas),
    yield takeLatest(actions.EDIT_SCAN_DATA_REQUEST, editScanDataSagas),
    yield takeLatest(actions.ADD_SCAN_DATA_REQUEST, addScanDataSagas),
    yield takeLatest(actions.DELETE_SCAN_DATA_REQUEST, deleteScanDataSagas),
    yield takeLatest(
      actions.DELETE_MULTI_SCAN_DATA_REQUEST,
      deleteMultiScanDataSagas
    ),
    yield takeLatest(actions.GET_QR_REQUEST, getQrSagas),
    yield takeLatest(actions.DOWNLOAD_SCAN_DATA_REQUEST, downloadScanDataSagas),
    yield takeEvery(actions.SEARCH_DATA_REQUEST, searchSaga),
    yield takeLatest(actions.GET_LIST_DATA_REQUEST, listScanDataSagas)
  ]);
}
