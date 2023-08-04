import { createAction, props } from '@ngrx/store';
import { IUserDto } from '../web-api-client';
//current app
export const setUser = createAction('[User] Set User', props<{ user: IUserDto }>());
// export const setUserRole = createAction('[Role] Set User Role', props<{ role: string }>());

