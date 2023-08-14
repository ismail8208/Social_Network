import { ChangeDetectionStrategy, Component, OnDestroy, OnInit, TemplateRef, } from '@angular/core';
import { Location } from '@angular/common';
import { Observable, combineLatest, combineLatestWith, map, tap, firstValueFrom, of } from 'rxjs';
import {
  CreateEducationCommand, CreateExperienceCommand, CreateSkillCommand,
  EducationsClient, ExperiencesClient, SkillsClient,
  ICreateEducationCommand, ICreateExperienceCommand, ICreateSkillCommand,
  IEducationDto, IExperienceDto, ISkillDto,
  IUpdateEducationCommand, IUpdateExperienceCommand, IUpdateSkillCommand,
  UpdateEducationCommand, UpdateExperienceCommand, UpdateSkillCommand,
  IUserDto,
  EndorsementsClient,
  IEndorsmentDto,
  UsersClient,
  FollowsClient,
  IBriefUserDto,
  IFollowCommand,
  FollowCommand,
  IUnFollowCommand,
  UnFollowCommand,
  IProjectDto,
  ProjectsClient,
  ICreateProjectCommand,
  CreateProjectCommand,
  IUpdateProjectCommand,
  UpdateProjectCommand,
  AddressesClient,
  NotificationsClient,
} from '../web-api-client';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { Store, select } from '@ngrx/store';
import { setUser } from '../stateManagement/user.actions';
import { selectUser } from '../stateManagement/user.selectors';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { LocalService } from '../sheard/localService';
import { NotificationServiceService } from '../sheard/notification-service.service';



@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ProfileComponent implements OnInit {


  isAuthenticated?: boolean;
  isFolloing: boolean = false;
  isLoaded: boolean = false;

  ifMember: boolean = false;

  user: IUserForSummary = {
    firstName: '',
    lastName: '',
    id: 0,
    numberOfFollowers: 0,
    profileImage: '',
    numberOfFollowings: 0,
    role: '',
    summary: '',
    userName: '',
    address: '',
    numberOfPosts: 0,

  };
  username: string;
  owner: IUserDto;
  isOwner: boolean;
  followAgree: boolean = false;

  skills: ISkillDto[] = [];
  educations: IEducationDto[] = [];
  experiences: IExperienceDto[] = [];
  projects: IProjectDto[] = [];


  filteredSKills: ISkillDto[] = [];
  filteredEducations: IEducationDto[] = [];
  filteredExperiences: IExperienceDto[] = [];
  filteredProjects: IExperienceDto[] = [];

  lenOfSkills: number = 0;
  lenOfEducations: number = 0;
  lenOfExperiences: number = 0;
  lenOfProjects: number = 0;

  showAllSkills: boolean = false;
  showAllEducations: boolean = false;
  showAllExperiences: boolean = false;
  showAllProjects: boolean = false;

  textskillbutton: string = '';
  textEducationbutton: string = '';
  textExperiencebutton: string = '';
  textProjectsbutton: string = '';

  isSkillsEnabled: boolean = false;
  isEducationsEnabled: boolean = false;
  isExperiencesEnabled: boolean = false;
  isProjectsEnabled: boolean = false;
  isAboutEnabled: boolean = false;

  public reactions: string[] = [];
  modalRef?: BsModalRef;



  constructor(
    private store: Store,
    private skillsClinet: SkillsClient,
    private educationsClient: EducationsClient,
    private authorizeService: AuthorizeService,
    private experiencesClient: ExperiencesClient,
    private projectsClient: ProjectsClient,
    private router: ActivatedRoute,
    private usersClient: UsersClient,
    private follows: FollowsClient,
    private modalService: BsModalService,
    private localService: LocalService,
    private notification: NotificationServiceService,
    private addressesClient: AddressesClient,
    private notificationsClient: NotificationsClient
  ) { }

  async ngOnInit() {
    this.isAuthenticated = await firstValueFrom(this.authorizeService.isAuthenticated());

    this.username = this.router.snapshot.paramMap.get('username');

    this.isOwner = this.localService.getData('username') === this.username;

    this.user = await firstValueFrom(this.usersClient.get(this.username).pipe(
      map(data => ({
        // ...data,
        profileImage: data.profileImage != null ? data.profileImage = `api/Images/${data.profileImage}` : data.profileImage = 'api/Images/logoimg.jpg',
        summary: data.summary != null ? data.summary : '',
        firstName: data.firstName ?? '',
        lastName: data.lastName ?? '',
        id: data.id ?? 0,
        numberOfFollowers: data.numberOfFollowers ?? 0,
        numberOfFollowings: data.numberOfFollowings ?? 0,
        role: data.role ?? '',
        userName: data.userName ?? '',
        address: '',
        specialization: data.specialization ?? '',
        numberOfPosts: data.numberOfPosts ?? 0
      })
      )
    ));

    this.ifMember=this.user.role == 'member';
     this.addressesClient.get(this.user.id).subscribe(
      {
        next: data => this.user.address = data.fullAddress
      }
    )

    this.isLoaded = true;
    this.checkIfUserFolloing();
    this.skillsClinet.getSkillsWithPagination(this.user.id, 1, 40).subscribe({
      next: (skills) => {
        this.skills = skills.items;
        this.lenOfSkills = skills.totalCount;
        this.textskillbutton = `Show all ${this.lenOfSkills} skills`;
      },
    });

    this.educationsClient.getEducationsWithPagination(this.user.id, 1, 40).subscribe(
      {
        next: data => {
          this.educations = data.items;
          this.lenOfEducations = data.totalCount;
          this.textEducationbutton = `Show all ${this.lenOfEducations} educations`;
        }
      });


    this.experiencesClient.getExperiencesWithPagination(this.user.id, 1, 40).subscribe(
      {
        next: data => {
          this.experiences = data.items;
          this.lenOfExperiences = data.totalCount;
          this.textExperiencebutton = `Show all ${this.lenOfExperiences} experiences`;
        }
      });

    this.projectsClient.getProjectsWithPagination(this.user.id, 1, 40).subscribe(
      {
        next: data => {
          this.projects = data.items;
          this.lenOfProjects = data.totalCount;
          this.textProjectsbutton = `Show all ${this.lenOfProjects} projects`;
        }
      });



  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }
  closeModal() {
    this.modalRef.hide();
  }

  reportBtn: string = 'Report'
  Report() {
    this.notificationsClient.report(parseInt(localStorage.getItem('id')), this.user.id).subscribe({
      next: data => {
        if (data) {
          this.reportBtn = 'Reported'
        }
      }
    })
  }
  deleteBtn: string = 'Delete'
  DeleteUser() {
    this.usersClient.delete(this.username).subscribe(
      {
        next: data => {
          if (data) {
            this.deleteBtn = 'Is Deleted';
          }
        }
      }
    )
  }
  ngOnDestroy(): void {
    // this.subSKill.unsubscribe();
    // this.subUser.unsubscribe();
    console.log("sun observable is finshed");
  }


  //Skills Methods Start
  enableSkillsSection() {
    console.log('any way today');
    this.isSkillsEnabled = true;
  }

  toggleSkills() {
    this.showAllSkills = !this.showAllSkills;
    this.textskillbutton = this.showAllSkills ? 'Hide' : `Show all ${this.lenOfSkills} skills`;

  }

  addSkill(skill: string) {
    console.log(`the skill ${skill} is added`);
    let entity: ICreateSkillCommand = {
      title: skill,
      userId: this.user.id
    }
    this.skillsClinet.create(entity as CreateSkillCommand).subscribe();
  }

  deleteSkill(id: number) {
    this.skillsClinet.delete(id).subscribe();
  }
  searchSkills(skill: string) {
    if(skill.length > 0) {
    this.skillsClinet.search(skill).subscribe({
      next: data =>
        this.filteredSKills = data.items
    });
  }
  }
  //Skills Methods End


  //Education Methods Start
  enableEducationsSection() {
    this.isEducationsEnabled = true;
  }

  toggleEducations() {
    this.showAllEducations = !this.showAllEducations;
    this.textEducationbutton = this.showAllEducations ? 'Hide' : `Show all ${this.lenOfEducations} educations`;
  }

  addEducation(education: IEducationDto) {
    let entity: ICreateEducationCommand = {
      title: education.title,
      level: education.level,
      userId: this.user.id
    }
    this.educationsClient.create(entity as CreateEducationCommand).subscribe()
  }

  edu: IEducationDto = {
    id: 0,
    title: 'empty',
    level: 'empty'
  };

  chosenEducation(education: IEducationDto) {
    this.edu = education;
  }

  updateEducation(education: IEducationDto) {
    let entity: IUpdateEducationCommand = {
      id: education.id,
      title: education.title,
      level: education.level,
      userId: this.user.id
    }
    this.educationsClient.update(education.id, entity as UpdateEducationCommand).subscribe();
  }

  searchEducation(education: string) {
    if(education.length > 0)
    this.educationsClient.search(education).subscribe({
      next: data =>
        this.filteredEducations = data.items
    });
  }

  deleteEducation(id: number) {
    this.educationsClient.delete(id).subscribe();
  }
  //Education Methods End

  //Projects Methods Start
  enableProjectsSection() {
    this.isProjectsEnabled = true;
  }

  toggleProjects() {
    this.showAllProjects = !this.showAllProjects;
    this.textProjectsbutton = this.showAllProjects ? 'Hide' : `Show all ${this.lenOfProjects} projects`;
  }

  addProject(project: IProjectDto) {
    let entity: ICreateProjectCommand = {
      title: project.title,
      description: project.description,
      link: project.link,
      userId: this.user.id,
    }
    this.projectsClient.create(entity as CreateProjectCommand).subscribe({
      next: data => {
        if (data > 0) {
          this.filteredProjects[data] = entity;
        }
      }
    })
  }

  pro: IProjectDto = {
    id: 0,
    title: 'empty',
    description: 'empty',
  };

  chosenProject(project: IProjectDto) {
    this.pro = project;
  }

  updateProject(project: IProjectDto) {
    let entity: IUpdateProjectCommand = {
      id: project.id,
      title: project.title,
      description: project.description,
      link: project.link,
      userId: this.user.id,
    }
    this.projectsClient.update(entity as UpdateProjectCommand).subscribe();
  }

  deleteProject(id: number) {
    this.projectsClient.delete(id).subscribe();
  }
  //Projects Methods End


  //Experiences Methods Start
  enableExperiencesSection() {
    this.isExperiencesEnabled = true;
  }

  toggleExperiences() {
    this.showAllExperiences = !this.showAllExperiences;
    this.textExperiencebutton = this.showAllExperiences ? 'Hide' : `Show all ${this.lenOfExperiences} experiences`;
  }

  addExperience(experience: IExperienceDto) {
    let entity: ICreateExperienceCommand = {
      title: experience.title,
      description: experience.description,
      experienceDate: experience.experienceDate,
      companyName: experience.companyName,
      startedTime: experience.startedTime,
      userId: this.user.id
    }
    this.experiencesClient.create(entity as CreateExperienceCommand).subscribe({
      next: data => {
        if (data > 0) {
          this.filteredExperiences[data] = entity;
        }
      }
    })
  }

  exper: IExperienceDto = {
    id: 0,
    title: 'empty',
    description: 'empty',
    // experienceDate: '2023-07-11T11:50:06.763Z'

  };

  chosenExperience(experience: IExperienceDto) {
    this.exper = experience;
  }

  updateExperience(experience: IExperienceDto) {
    let entity: IUpdateExperienceCommand = {
      id: experience.id,
      title: experience.title,
      description: experience.description,
      experienceDate: experience.experienceDate,
      companyName: experience.companyName,
      startedTime: experience.startedTime,
      userId: this.user.id
    }
    this.experiencesClient.update(experience.id, entity as UpdateExperienceCommand).subscribe();
  }

  searchExperience(experience: string) {
  }

  deleteExperience(id: number) {
    this.experiencesClient.delete(id).subscribe();
  }

  //Experince Methods End 

  //About start
  enableAboutSection() {
    this.isAboutEnabled = true;
  }
  //About end

  follow() {
    let entity: IFollowCommand =
    {
      userId: parseInt(this.localService.getData('id')),
      specificUserId: this.user.id
    }
    this.follows.follow(entity as FollowCommand).subscribe(
      {
        next: data => {
          if (data > 0) {
            this.followAgree = false;
            this.isFolloing = false;
          }
          else {
            this.followAgree = true;
            this.isFolloing = true;
          }
        }
      }
    )
  }
  unFollow() {
    console.log('hello world');
    let entity: IUnFollowCommand =
    {
      userId: parseInt(this.localService.getData('id')),
      specificUserId: this.user.id
    }
    this.follows.unFollow(entity as UnFollowCommand).subscribe(
      {
        next: data => {
          if (data > 0) {
            this.followAgree = true;
            this.isFolloing = true;
          }
          else {
            this.followAgree = false;
            this.isFolloing = false;
          }
        }
      }
    )
  }


  checkIfUserFolloing(): any {
    this.follows.getFollowersWithPagination(this.user.id, 1, this.user.numberOfFollowers).subscribe(
      {
        next: data => {
          const f = data.items.find(u => u.userName == localStorage.getItem('username'));
          if (f) {
            this.isFolloing = true;
          }
          else this.isFolloing = false;
        }
      }
    )
  }

}
interface IUserForSummary extends IUserDto {
  address: string,

}