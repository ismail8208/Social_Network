import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { selectUser } from 'src/app/stateManagement/user.selectors';
import { CreateEndorsementCommand, EndorsementsClient, ICreateEndorsementCommand, IEndorsmentDto, ISkillDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-skill',
  templateUrl: './skill.component.html',
  styleUrls: ['./skill.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SkillComponent implements OnInit {

  listOfEndorsements: IEndorsmentDto[];
  userId: number;
  isEndorsed = false;
  endorseId: number;
  @Input() isOwner: boolean;
  
  @Input() skill: ISkillDto; 
   
  @Output() skillForDeleted: EventEmitter<number> = new EventEmitter<number>();
  constructor(
    private endorsementsClient: EndorsementsClient,
    private store: Store,
  ){}

  ngOnInit(): void {

    this.store.pipe(select(selectUser)).subscribe({
      next: (data) => {
        if (data) {
          this.userId = data.id;
        }
      },
    });    
    this.endorsementsClient.getEndorsmentsWithPagination(this.skill.id, 1, 40).subscribe(
      {
        next: data => this.listOfEndorsements = data.items
      }
    );
  }

  
  Endorsement(){
    if(!this.isEndorsed) 
    {
    let entity : ICreateEndorsementCommand = {
        skillId: this.skill.id,
        userId: this.userId
    }
    this.endorsementsClient.create(entity as CreateEndorsementCommand).subscribe();

    this.endorsementsClient.getEndorsmentsWithPagination(this.skill.id, 1, 40).subscribe(
      {
        next: data => this.listOfEndorsements = data.items
      }
    );

   } else {
    let endorse = this.listOfEndorsements.find(e => {e.userId == this.userId})
    this.endorsementsClient.delete(endorse.id).subscribe();
    this.listOfEndorsements.pop();

   }
  }

  toggleEndorsement()
  {
    this.isEndorsed = !this.isEndorsed;
  }
  deleteSkill()
  {
   this.skillForDeleted.emit(this.skill.id);
  }
}