import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import { FollowsClient, IBriefUserDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-followings',
  templateUrl: './followings.component.html',
  styleUrls: ['./followings.component.css']
})
export class FollowingsComponent implements OnInit {

  followings: IBriefUserDto[] = [];
  id: number;
  constructor(private router: ActivatedRoute, private follows: FollowsClient) { }

  ngOnInit() {
    this.id = parseInt(this.router.snapshot.paramMap.get('id'));
    this.follows.getFollowingsWithPagination(this.id, 1, 50).pipe(
      map(followings => followings.items.map(following => ({
        ...following,
        profileImage: following.profileImage != null ? `https://localhost:44447/api/Images/${following.profileImage}` : 'https://localhost:44447/api/Images/f08c0eb9-cdde-471c-af59-a83005ea784f_Screenshot_٢٠٢٠-٠٩-٢٠-١٦-٤٤-١١.png'

      })))
    ).subscribe({
      next: data => {
        this.followings = data
      }
    })
  }

}
