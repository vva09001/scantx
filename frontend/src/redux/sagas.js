import { all } from 'redux-saga/effects';
import authSagas from './auth/saga';
import userSagas from './user/saga';
import dataSagas from './data/saga';
import companySagas from './company/saga';
import scanDataSagas from './scandata/saga';

export default function* rootSaga(getState) {
	yield all([authSagas(), userSagas(), dataSagas(), companySagas(), scanDataSagas()]);
}
