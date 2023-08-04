import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IEducation, IEducationDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-add-education',
  templateUrl: './add-education.component.html',
  styleUrls: ['./add-education.component.css']
})
export class AddEducationComponent {

  @Output() selectdEducation: EventEmitter<IEducationDto> = new EventEmitter<IEducationDto>();

  @Output() _listFilter: EventEmitter<string> = new EventEmitter<string>(); //output
  
  @Input() filteredEducations : IEducationDto[] = []; //input

  v: string = '';
  level: string = '';

  public set listFilter(v : string) {
      this._listFilter.emit(v);
      this.v = v;
  }
  
  chosenEducation: string ='';
  chooseEducation(education: IEducationDto) {
      this.chosenEducation = education.title;
  }

  openD() {
    const modal = document.querySelector('.modalDialogAE') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }
  
  closeD() { 
    const modal = document.querySelector('.modalDialogAE') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
    this.filteredEducations = [];
    this.chosenEducation = '';
    this.level ='';
    this.listFilter='';
  }

  saveEducation()
  {
    let entity: IEducationDto = {
      title: this.chosenEducation == '' ? this.v : this.chosenEducation,
      level: this.level
    }
    this.v !='' &&  this.selectdEducation.emit(entity);
    this.filteredEducations = [];
    this.chosenEducation = '';
    this.level ='';
    this.listFilter='';
  }
}
