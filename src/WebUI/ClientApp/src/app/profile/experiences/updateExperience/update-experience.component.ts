import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IExperienceDto, IExperience } from 'src/app/web-api-client';

@Component({
  selector: 'app-update-experience',
  templateUrl: './update-experience.component.html',
  styleUrls: ['./update-experience.component.css']
})
export class UpdateExperienceComponent {

  @Input() experience?: IExperienceDto;
  @Output() chosenExperience: EventEmitter<IExperienceDto> = new EventEmitter<IExperienceDto>();

  closeD() {
    const modal = document.querySelector('.modalDialogUEX') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
  }

  updateExperience() {
    this.chosenExperience.emit(this.experience);
  }
}
