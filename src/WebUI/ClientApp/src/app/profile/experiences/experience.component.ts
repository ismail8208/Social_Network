import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IExperienceDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-experience',
  templateUrl: './experience.component.html',
  styleUrls: ['./experience.component.css']
})
export class ExperienceComponent {

  @Input() experience?: IExperienceDto;

  @Output() experienceForUpdate: EventEmitter<IExperienceDto> = new EventEmitter<IExperienceDto>();

  @Output() experienceForDeleted: EventEmitter<number> = new EventEmitter<number>();
  
  deleteExperience()
  {
   this.experienceForDeleted.emit(this.experience!.id);
   console.log(this.experience!.id)
  }

  openD() {
    const modal = document.querySelector('.modalDialogUEX') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }

  send()
  {
    this.experienceForUpdate.emit(this.experience)
  }
}
