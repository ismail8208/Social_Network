import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IEducation, IEducationDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-update-education',
  templateUrl: './update-education.component.html',
  styleUrls: ['./update-education.component.css']
})
export class UpdateEducationComponent {

  @Input() education?: IEducationDto;
  @Output() chosenEducation: EventEmitter<IEducationDto> = new EventEmitter<IEducation>();

  closeD(){
    const modal = document.querySelector('.modalDialogUE') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
  }

  updateEducation()
  {
    this.chosenEducation.emit(this.education);
  }
}
