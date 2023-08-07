import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IProjectDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent {

  @Input() project?: IProjectDto;
  @Input() isOwner: boolean;

  @Output() projectForUpdate: EventEmitter<IProjectDto> = new EventEmitter<IProjectDto>();

  @Output() projectForDeleted: EventEmitter<number> = new EventEmitter<number>();

  deleteproject() {
    this.projectForDeleted.emit(this.project!.id);
    console.log(this.project!.id)
  }

  openD() {
    const modal = document.querySelector('.modalDialogUP') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }

  send() {
    this.projectForUpdate.emit(this.project)
  }

}
