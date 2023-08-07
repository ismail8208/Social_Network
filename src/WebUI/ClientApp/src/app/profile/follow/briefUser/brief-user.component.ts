import { Component, Input, OnInit } from '@angular/core';
import { CancelFollowerCommand, FollowCommand, FollowsClient, IBriefUserDto, ICancelFollowerCommand, IFollowCommand } from 'src/app/web-api-client';

@Component({
  selector: 'app-brief-user',
  templateUrl: './brief-user.component.html',
  styleUrls: ['./brief-user.component.css']
})
export class BriefUserComponent implements OnInit {

  @Input() user: IBriefUserDto;
  isFollowMe: boolean;
  itsMe: boolean;
  
  constructor(private follows: FollowsClient) { }

  ngOnInit(): void {

    this.itsMe = this.user.userName != localStorage.getItem('username');
    this.follows.getFollowingsWithPagination(this.user.id, 1, 50).subscribe(
      {
        next: data => {
          const f = data.items.find(u => u.userName == localStorage.getItem('username'));
          if (f) {
            this.isFollowMe = true;
          }
          else
          {
           this.isFollowMe = false;
          }
          
        }
      }
    )
  }

  toggleBtn() {
    if(this.isFollowMe)
    {
      this.isFollowMe = false;
    }
  }

  toggleState() {
    if (!this.isFollowMe) {
      let entity: ICancelFollowerCommand = {
        userId: parseInt(localStorage.getItem('id')),
        specificUserId: this.user.id
      }
      this.follows.cancelFollower(entity as CancelFollowerCommand).subscribe();
    }
    // else {
    //   let entity: IFollowCommand =
    //   {
    //     userId: parseInt(localStorage.getItem('id')),
    //     specificUserId: this.user.id
    //   }
    //   this.follows.follow(entity as FollowCommand).subscribe();
    // }
  }

}
