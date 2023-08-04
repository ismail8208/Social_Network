import { createFeatureSelector, createSelector } from '@ngrx/store';
import { UserState } from './user.reducer';
import { IUserDto } from '../web-api-client';

// Get the user feature state from the root state
export const selectUserState = createFeatureSelector<UserState>('user');

// Get the user from the user feature state
export const selectUser = createSelector(
  selectUserState,
  (state: UserState) => state.user
);

// export const selectUserRoleState = createFeatureSelector<UserRoleState>('role');


// export const selectUserRole = createSelector(
//   selectUserRoleState,
//   (state: UserRoleState) => state.role
// );