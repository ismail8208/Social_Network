import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap/modal';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { TodoComponent } from './todo/todo.component';
import { TokenComponent } from './token/token.component';

import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProfileComponent } from './profile/profile.component';
import {StoreModule } from '@ngrx/store';
import { AboutComponent } from './profile/about/about.component';
import { EducationComponent } from './profile/educations/education.component';
import { AddEducationComponent } from './profile/educations/addEducation/add-education.component';
import { UpdateEducationComponent } from './profile/educations/updateEducation/update-education.component';
import { SkillComponent } from './profile/skills/skill.component';
import { AddSkillComponent } from './profile/skills/addSkill/add-skill.component';
import { userReducer } from './stateManagement/user.reducer';
import { ExperienceComponent } from './profile/experiences/experience.component';
import { AddExperienceComponent } from './profile/experiences/addExperience/add-experience.component';
import { UpdateExperienceComponent } from './profile/experiences/updateExperience/update-experience.component';
import { converToRoot } from './sheard/conver-to-root.pip';
import { PostComponent } from './home/post/post.component';
import { AddPostCardComponent } from './home/addPostCard/addPostCard.component';
import { ConvertToUrl } from './sheard/convert-to-url';
import { FollowersComponent } from './profile/follow/followers.component';
import { FollowingsComponent } from './profile/follow/followings.component';
import { SearchUserComponent } from './profile/search-user/search-user.component';
import { BriefUserComponent } from './profile/follow/briefUser/brief-user.component';
import { ProjectComponent } from './profile/projects/project.component';
import { UpdateProjectComponent } from './profile/projects/update-project/update-project.component';
import { AddProjectComponent } from './profile/projects/add-project/add-project.component';
import { WritePostComponent } from './profile/posts/write-post/write-post.component';
import { ExportCvComponent } from './profile/export-cv/export-cv.component';
import { HasRoleDirective } from './has-role.directive';
import { WriteJobComponent } from './profile/jobs/write-job/write-job.component';
import { JobCardComponent } from './profile/jobs/job-card/job-card.component';
import { JobsListComponent } from './profile/jobs/jobs-list/jobs-list.component';
import { SubProfileComponent } from './home/subProfile/subProfile.component';
import { StateJobListComponent } from './profile/cv-service/state-job-list/state-job-list.component';
import { CvListComponent } from './profile/cv-service/cv-list/cv-list.component';
import { BriefPostsComponent } from './profile/posts/brief-posts/brief-posts.component';
import { MyPostComponent } from './profile/posts/myPost/myPost.component';
import { ConvertToSettings } from './sheard/convert-to-settings';
import { NotificationsComponent } from './profile/notifications/notifications.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { UsersChartsComponent } from './Admin/dashboard/users-charts/users-charts.component';
import { PostsChartsComponent } from './Admin/dashboard/posts-charts/posts-charts.component';
import { JobsChartsComponent } from './Admin/dashboard/jobs-charts/jobs-charts.component';
import { AdvertisementsComponent } from './home/advertisements/advertisements.component';
import { LikesListComponent } from './home/likes-list/likes-list.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    TodoComponent,
    TokenComponent,
    ProfileComponent,
    AboutComponent,
    EducationComponent,
    AddEducationComponent,
    UpdateEducationComponent,
    SkillComponent,
    AddSkillComponent,
    ExperienceComponent,
    AddExperienceComponent,
    UpdateExperienceComponent,
    converToRoot,
    PostComponent,
    MyPostComponent,
    AddPostCardComponent,
    ConvertToUrl,
    ConvertToSettings,
    FollowersComponent,
    FollowingsComponent,
    SearchUserComponent,
    BriefUserComponent,
    ProjectComponent,
    UpdateProjectComponent,
    AddProjectComponent,
    WritePostComponent,
    ExportCvComponent,
    HasRoleDirective,
    WriteJobComponent,
    JobCardComponent,
    JobsListComponent,
    HasRoleDirective,
    SubProfileComponent,
    AdvertisementsComponent,
    StateJobListComponent,
    CvListComponent,
    BriefPostsComponent,
    NotificationsComponent,
    DashboardComponent,
    UsersChartsComponent,
    PostsChartsComponent,
    JobsChartsComponent,
    LikesListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ModalModule.forRoot(),
    StoreModule.forRoot({ user: userReducer})
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
