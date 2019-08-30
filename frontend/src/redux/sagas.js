import { all } from 'redux-saga/effects';
import authSagas from './auth/saga';
import userSagas from './user/saga';
import companySagas from './company/saga';

export default function* rootSaga(getState) {
	yield all([authSagas(), userSagas(), companySagas()]);
}
