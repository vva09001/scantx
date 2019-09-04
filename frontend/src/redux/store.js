import { createStore, combineReducers, applyMiddleware, compose } from 'redux';
import { persistStore, persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage'
import createHistory from 'history/createBrowserHistory';
import { routerReducer, routerMiddleware } from 'react-router-redux';
import createSagaMiddleware from 'redux-saga';
import reducers from '../redux/reducers';
import rootSaga from '../redux/sagas';

const history = createHistory();
const sagaMiddleware = createSagaMiddleware();
const routeMiddleware = routerMiddleware(history);
const middlewares = [sagaMiddleware, routeMiddleware];
const rootReducer = combineReducers({
  ...reducers,
  router: routerReducer
});

const persistConfig = {
  key: 'root',
  storage,
  whitelist: ['Auth']
};
const persistedReducer = persistReducer(persistConfig, rootReducer)


const store = createStore(
  persistedReducer,
  compose(applyMiddleware(...middlewares))
);
const persistor = persistStore(store)
sagaMiddleware.run(rootSaga);
export { store, persistor, history };
