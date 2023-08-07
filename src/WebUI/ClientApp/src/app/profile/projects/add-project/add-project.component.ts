import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IProjectDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.css']
})
export class AddProjectComponent {

  @Output() selectdProject: EventEmitter<IProjectDto> = new EventEmitter<IProjectDto>();

  @Output() _listFilter: EventEmitter<string> = new EventEmitter<string>(); //output

  @Input() filteredProjects: IProjectDto[] = []; //input

  v: string = '';
  description: string = '';
  link: string;



  public set listFilter(v: string) {
    this._listFilter.emit(v);
    this.v = v;
  }


  chosenProject: string = '';
  chooseProject(project: IProjectDto) {
    this.chosenProject = project.title;
  }

  openD() {
    const modal = document.querySelector('.modalDialogAP') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }

  closeD() {
    const modal = document.querySelector('.modalDialogAP') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
    this.chosenProject = '';
    this.filteredProjects = [];
    this.listFilter = '';
    this.description = '';
  }

  saveProject() {
    let entity: IProjectDto = {
      title: this.chosenProject == '' ? this.v : this.chosenProject,
      description: this.description,
      link: this.link,
    }
    this.v != ' ' && this.selectdProject.emit(entity);
    this.chosenProject = '';
    this.filteredProjects = [];
    this.listFilter = '';
    this.description = '';
    this.link = '';
  }

}
