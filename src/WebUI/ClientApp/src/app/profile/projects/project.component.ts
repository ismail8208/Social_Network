import { Component, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { IProjectDto, ProjectsClient } from 'src/app/web-api-client';

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
  
  deleteProjectModalRef: BsModalRef;
  projectIdForDelete: number;
  constructor(
   
    private projectsClient: ProjectsClient,
 
    private modalService: BsModalService,
  

  ) { }

  showDeleteProjectModal(template: TemplateRef<any>): void {
    this.deleteProjectModalRef = this.modalService.show(template);
    this.projectIdForDelete = this.project.id;
  }

  deleteProjectCancelled(): void {
    this.deleteProjectModalRef.hide();
  }

  deleteProject(): void {
    this.projectsClient.delete(this.projectIdForDelete).subscribe(error => console.error(error));

    this.deleteProjectCancelled();
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
