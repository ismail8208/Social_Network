import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { TodoComponent } from './todo/todo.component';
import { TokenComponent } from './token/token.component';
import { ProfileComponent } from './profile/profile.component';
import { FollowersComponent } from './profile/follow/followers.component';
import { FollowingsComponent } from './profile/follow/followings.component';
import { ExportCvComponent } from './profile/export-cv/export-cv.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent/*, pathMatch: 'full'*/, canActivate: [AuthorizeGuard] },
  { path: 'followers/:id', component: FollowersComponent /*, pathMatch: 'full'*/, canActivate: [AuthorizeGuard] },
  { path: 'followings/:id', component: FollowingsComponent /*, pathMatch: 'full'*/, canActivate: [AuthorizeGuard] },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },

  { path: 'profile/:username', component: ProfileComponent, canActivate: [AuthorizeGuard] },
  { path: 'cv/:id', component: ExportCvComponent},
  { path: 'todo', component: TodoComponent, canActivate: [AuthorizeGuard] },
  { path: 'token', component: TokenComponent, canActivate: [AuthorizeGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
