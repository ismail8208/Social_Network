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
import { StateJobListComponent } from './profile/cv-service/state-job-list/state-job-list.component';
import { CvListComponent } from './profile/cv-service/cv-list/cv-list.component';
import { MyPostComponent } from './profile/posts/myPost/myPost.component';
import { NotificationsComponent } from './profile/notifications/notifications.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { UsersChartsComponent } from './Admin/dashboard/users-charts/users-charts.component';
import { PostsChartsComponent } from './Admin/dashboard/posts-charts/posts-charts.component';
import { JobsChartsComponent } from './Admin/dashboard/jobs-charts/jobs-charts.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent/*, pathMatch: 'full'*/, canActivate: [AuthorizeGuard] },
  { path: 'followers/:id', component: FollowersComponent /*, pathMatch: 'full'*/, canActivate: [AuthorizeGuard] },
  { path: 'followings/:id', component: FollowingsComponent /*, pathMatch: 'full'*/, canActivate: [AuthorizeGuard] },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
//notifications
//Dashboard

  { path: 'notifications', component: NotificationsComponent, canActivate: [AuthorizeGuard] },
  { path: 'users-chart', component: UsersChartsComponent, canActivate: [AuthorizeGuard] },
  { path: 'posts-chart', component: PostsChartsComponent, canActivate: [AuthorizeGuard] },
  { path: 'jobs-chart', component: JobsChartsComponent, canActivate: [AuthorizeGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthorizeGuard] },
  { path: 'profile/:username', component: ProfileComponent, canActivate: [AuthorizeGuard] },
  { path: 'job-list', component: StateJobListComponent, canActivate: [AuthorizeGuard] },
  { path: 'CVs-job/:jobId', component: CvListComponent, canActivate: [AuthorizeGuard] },
  { path: 'cv/:id', component: ExportCvComponent},
  { path: 'todo', component: TodoComponent, canActivate: [AuthorizeGuard] },
  { path: 'my-posts/:id', component: MyPostComponent, canActivate: [AuthorizeGuard] },
  { path: 'token', component: TokenComponent, canActivate: [AuthorizeGuard] },
  { path: '', redirectTo: 'home', pathMatch: 'full'},
  { path: '**', redirectTo: 'home', pathMatch: 'full'},

];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
