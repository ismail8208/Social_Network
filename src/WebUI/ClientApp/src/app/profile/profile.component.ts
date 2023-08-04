import { ChangeDetectionStrategy, Component, OnDestroy, OnInit, TemplateRef, } from '@angular/core';
import { Location } from '@angular/common';
import { Observable, combineLatest, combineLatestWith, map, tap, firstValueFrom } from 'rxjs';
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
  FollowCommand
} from '../web-api-client';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { Store, select } from '@ngrx/store';
import { setUser } from '../stateManagement/user.actions';
import { selectUser } from '../stateManagement/user.selectors';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';



@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ProfileComponent implements OnInit {



  isAuthenticated?: boolean;
  user: IUserDto | undefined;
  username: string;
  owner: IUserDto;
  isOwner: boolean;

  skills: ISkillDto[] = [];
  educations: IEducationDto[] = [];
  experiences: IExperienceDto[] = [];

  filteredSKills: ISkillDto[] = [];
  filteredEducations: IEducationDto[] = [];
  filteredExperiences: IExperienceDto[] = [];

  lenOfSkills: number = 0;
  lenOfEducations: number = 0;
  lenOfExperiences: number = 0;

  showAllSkills: boolean = false;
  showAllEducations: boolean = false;
  showAllExperiences: boolean = false;

  textskillbutton: string = '';
  textEducationbutton: string = '';
  textExperiencebutton: string = '';

  isSkillsEnabled: boolean = false;
  isEducationsEnabled: boolean = false;
  isExperiencesEnabled: boolean = false;
  isAboutEnabled: boolean = false;

  public reactions: string[] = [];
  public bsModalRef: BsModalRef;
  redirectDetailsModalRef: BsModalRef;


  constructor(
    private store: Store,
    private skillsClinet: SkillsClient,
    private educationsClient: EducationsClient,
    private authorizeService: AuthorizeService,
    private experiencesClient: ExperiencesClient,
    private router: ActivatedRoute,
    private usersClient: UsersClient,
    private follows: FollowsClient,
    private modalService: BsModalService,
  ) { }

  async ngOnInit() {
    this.isAuthenticated = await firstValueFrom(this.authorizeService.isAuthenticated());
    this.owner = await firstValueFrom(this.store.pipe(select(selectUser)));
    // , tap(item => this.isOwner = item.userName === this.router.snapshot.paramMap.get('username'))
    // this.store.pipe(select(selectUser)).subscribe({
    //   next: (data) => {
    //     if (data) {
    //       this.owner = data;
    //     }
    //   },
    // });

    this.username = this.router.snapshot.paramMap.get('username');
    this.isOwner = true;
    // this.isOwner = this.owner.userName === this.username;

    this.usersClient.get(this.username).subscribe(
      {
        next: data => {
          this.user = data;
          if (data.profileImage) {
            this.user.profileImage = `https://localhost:44447/api/Images/${data.profileImage}`;
          }
          else {
            this.user.profileImage = 'https://localhost:44447/api/Images/2b388861-8663-4843-9f65-5481388e927d_Screenshot 2023-05-06 211008.png';
          }
        }
      }
    )

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



  }

  showListOptionsModal(template: TemplateRef<any>) {
    this.redirectDetailsModalRef = this.modalService.show(template);
  }

  // ngOnDestroy(): void {
  //     // this.subSKill.unsubscribe();
  //     // this.subUser.unsubscribe();
  //     console.log("sun observable is finshed");
  // }


  //Skills Methods Start
  enableSkillsSection() {
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
    this.skillsClinet.create(entity as CreateSkillCommand).subscribe()
  }

  deleteSkill(id: number) {
    this.skillsClinet.delete(id).subscribe();
  }
  searchSkills(skill: string) {
    this.skillsClinet.search(skill).subscribe({
      next: data =>
        this.filteredSKills = data.items
    });
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
    this.educationsClient.search(education).subscribe({
      next: data =>
        this.filteredEducations = data.items
    });
  }

  deleteEducation(id: number) {
    this.educationsClient.delete(id).subscribe();
  }
  //Education Methods End

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
      userId: this.user.id
    }
    this.experiencesClient.create(entity as CreateExperienceCommand).subscribe()
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
      userId: this.user.id
    }
    this.experiencesClient.update(experience.id, entity as UpdateExperienceCommand).subscribe();
  }

  searchExperience(experience: string) {
  }

  deleteExperience(id: number) {
    this.experiencesClient.delete(id).subscribe();
  }
  //Experiences Methods End

  //About start
  enableAboutSection() {
    this.isAboutEnabled = true;
  }
  //About end

  follow() {
    let entity: IFollowCommand =
    {
      userId: this.owner.id,
      specificUserId: this.user.id
    }
    this.follows.follow(entity as FollowCommand).subscribe()
  }

  close() {
    this.redirectDetailsModalRef.hide();
  }

}


   // setTimeout(() => {

    // this.endorsementsClient.getEndorsmentsWithPagination(1016, 1, 40).pipe(

    //   combineLatestWith(
    //     this.skillsClinet.getSkillsWithPagination(this.user.id, 1, 40).pipe(
    //     tap(data => {
    //       this.lenOfSkills = data.totalCount;
    //       this.textskillbutton =  `Show all ${this.lenOfSkills} skills`
    //     }))),

    //   map(([CLendorsements, CLskills]) =>  CLskills.items.map(skill => ({
    //     ...skill,
    //     listOfEndorsements: CLendorsements.items.filter(e => skill.id == e.skillId)
    //   } as ISkill)))).subscribe({
    //     next: data =>  {
    //       this.skills = data;
    //     }
    //   });
    // }, 500);
