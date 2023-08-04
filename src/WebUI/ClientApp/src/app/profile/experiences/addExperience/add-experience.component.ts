import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IExperienceDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-add-experience',
  templateUrl: './add-experience.component.html',
  styleUrls: ['./add-experience.component.css']
})
export class AddExperienceComponent {

  
  @Output() selectdExperience: EventEmitter<IExperienceDto> = new EventEmitter<IExperienceDto>();

  @Output() _listFilter: EventEmitter<string> = new EventEmitter<string>(); //output
  
  @Input() filteredExperiences : IExperienceDto[] = []; //input

  v: string = '';
  description: string = '';


  public set listFilter(v : string) {
      this._listFilter.emit(v);
      this.v = v;
  }
  
  
  chosenExperience: string ='';
  chooseExperience(experience: IExperienceDto) {
      this.chosenExperience = experience.title;
  }

  openD() {
    const modal = document.querySelector('.modalDialogAEX') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }
  
  closeD(){
    const modal = document.querySelector('.modalDialogAEX') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
    this.chosenExperience='';
    this.filteredExperiences = [];
    this.listFilter='';
    this.description='';
  }

  saveExperience()
  {
    let entity: IExperienceDto = {
      title: this.chosenExperience == '' ? this.v : this.chosenExperience,
      description: this.description
    }
      this.v !=' ' &&  this.selectdExperience.emit(entity);
      this.chosenExperience='';
      this.filteredExperiences = [];
      this.listFilter='';
      this.description='';
  }
}
