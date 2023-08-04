import { combineReducers } from '@ngrx/store';
import { userReducer, userRoleReducer } from './user.reducer';

export const rootReducer = combineReducers({
  user: userReducer,
  role: userRoleReducer
});