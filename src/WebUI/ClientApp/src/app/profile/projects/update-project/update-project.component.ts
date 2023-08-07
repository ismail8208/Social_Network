import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IProjectDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-update-project',
  templateUrl: './update-project.component.html',
  styleUrls: ['./update-project.component.css']
})
export class UpdateProjectComponent {

  @Input() project?: IProjectDto;
  @Output() chosenProject: EventEmitter<IProjectDto> = new EventEmitter<IProjectDto>();

  closeD() {
    const modal = document.querySelector('.modalDialogUP') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
  }

  updateProject() {
    this.chosenProject.emit(this.project);
  }
}
