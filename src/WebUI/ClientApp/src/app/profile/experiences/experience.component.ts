import { Component, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ExperiencesClient, IExperienceDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-experience',
  templateUrl: './experience.component.html',
  styleUrls: ['./experience.component.css']
})
export class ExperienceComponent {


  deleteExperienceModalRef: BsModalRef
  experienceIdForDelete: number;

  @Input() experience?: IExperienceDto;
  @Input() isOwner: boolean;

  @Output() experienceForUpdate: EventEmitter<IExperienceDto> = new EventEmitter<IExperienceDto>();

  @Output() experienceForDeleted: EventEmitter<number> = new EventEmitter<number>();

  constructor(
    private experiencesClient: ExperiencesClient,
    private modalService: BsModalService,

  ) { }
  showDeleteExperienceModal(template: TemplateRef<any>): void {
    this.deleteExperienceModalRef = this.modalService.show(template);
    this.experienceIdForDelete = this.experience.id;
  }

  deleteExperienceCancelled(): void {
    this.deleteExperienceModalRef.hide();
  }

  deleteExperience(): void {
    this.experiencesClient.delete(this.experienceIdForDelete).subscribe(error => console.error(error));

    this.deleteExperienceCancelled();
  }

  openD() {
    const modal = document.querySelector('.modalDialogUEX') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }

  send() {
    this.experienceForUpdate.emit(this.experience)
  }
}
