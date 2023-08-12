import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import { LocalService } from 'src/app/sheard/localService';
import { FollowsClient, IBriefUserDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-followings',
  templateUrl: './followings.component.html',
  styleUrls: ['./followings.component.css']
})
export class FollowingsComponent implements OnInit {

  followings: IBriefUserDto[] = [];
  id: number;
  isFollowMe: boolean;
  constructor(private router: ActivatedRoute, private follows: FollowsClient, private localService: LocalService) { }

  ngOnInit() {
    this.id = parseInt(this.router.snapshot.paramMap.get('id'));
    this.follows.getFollowingsWithPagination(this.id, 1, 50).pipe(
      map(followings => followings.items.map(following => ({
        ...following,
        profileImage: following.profileImage != null ? `api/Images/${following.profileImage}` : 'api/Images/logoimg.jpg'

      })))
    ).subscribe({
      next: data => {
        this.followings = data;
        const f = data.find(u => u.userName == this.localService.getData('username'));
        if (f) {
          this.isFollowMe = false;
        }
        else this.isFollowMe = true;
      }
    })
  }

  CancelFollower() {

  }

  toggleBtn() {

  }

}
