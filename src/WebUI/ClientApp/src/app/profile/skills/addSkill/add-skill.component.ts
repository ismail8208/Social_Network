import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ISkillDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-add-skill',
  templateUrl: './add-skill.component.html',
  styleUrls: ['./add-skill.component.css']
})
export class AddSkillComponent{

  @Output() selectedSKill: EventEmitter<string> = new EventEmitter<string>();

  @Output() _listFilter: EventEmitter<string> = new EventEmitter<string>(); //output
  
  @Input() filteredSKills : ISkillDto[] = []; //input

  v: string = '';

  public set listFilter(v : string) {
      this._listFilter.emit(v);
      this.v = v;
  }
  
  openD() {
    const modal = document.querySelector('.modalDialogAS') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
    this.listFilter='';
  }
  
  closeD(){
    const modal = document.querySelector('.modalDialogAS') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
    this.filteredSKills = [];
  }
  chosenSkill: string ='';
  chooseSkill(skill: ISkillDto) {
      this.chosenSkill = skill.title;
  }

  saveSkill()
  {
      this.v !=' ' &&  this.selectedSKill.emit((this.chosenSkill == '' ? this.v : this.chosenSkill));
      this.chosenSkill = '';
  }

}
