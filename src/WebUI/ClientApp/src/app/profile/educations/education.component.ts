import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IEducation, IEducationDto } from 'src/app/web-api-client';

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
  
  deleteEducation()
  {
   this.educationForDeleted.emit(this.education!.id);
   console.log(this.education!.id)
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
