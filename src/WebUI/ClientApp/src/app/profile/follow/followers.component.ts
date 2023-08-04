import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import { FollowsClient, IBriefUserDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-follow',
  templateUrl: './followers.component.html',
  styleUrls: ['./followers.component.css']
})
export class FollowersComponent implements OnInit {

  followers: IBriefUserDto[] = [];
  id: number;

  constructor(private router: ActivatedRoute, private follows: FollowsClient) { }

  ngOnInit() {
    this.id = parseInt(this.router.snapshot.paramMap.get('id'));
    this.follows.getFollowersWithPagination(this.id, 1, 50).pipe(
      map(followers => followers.items.map(follower => ({
        ...follower,
        profileImage: follower.profileImage != null ? `https://localhost:44447/api/Images/${follower.profileImage}` : 'https://localhost:44447/api/Images/f08c0eb9-cdde-471c-af59-a83005ea784f_Screenshot_٢٠٢٠-٠٩-٢٠-١٦-٤٤-١١.png'

      })))
    ).subscribe({
      next: data => {
        this.followers = data
      }
    })
  }

}
