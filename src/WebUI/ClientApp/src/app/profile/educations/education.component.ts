import { Component, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EducationsClient, IEducation, IEducationDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-education',
  templateUrl: './education.component.html',
  styleUrls: ['./education.component.css']
})
export class EducationComponent {

  @Input() education?: IEducationDto;
  @Input() isOwner: boolean;

  @Output() educationForUpdate: EventEmitter<IEducationDto> = new EventEmitter<IEducationDto>();

  @Output() educationForDeleted: EventEmitter<number> = new EventEmitter<number>();
  

  deleteEducationModalRef: BsModalRef
  educationIdForDelete: number;

  constructor(
    private educationsClient: EducationsClient,
    private modalService: BsModalService,

  ) { }
  showDeleteEducationModal(template: TemplateRef<any>): void {
    this.deleteEducationModalRef = this.modalService.show(template);
    this.educationIdForDelete = this.education.id;
  }

  deleteEducationCancelled(): void {
    this.deleteEducationModalRef.hide();
  }

  deleteEducation(): void {
    this.educationsClient.delete(this.educationIdForDelete).subscribe(error => console.error(error));

    this.deleteEducationCancelled();
  }

  openD() {
    const modal = document.querySelector('.modalDialogUE') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }

  send()
  {
    this.educationForUpdate.emit(this.education)
  }
}
