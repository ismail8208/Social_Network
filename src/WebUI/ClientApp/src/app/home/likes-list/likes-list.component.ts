import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import { FollowsClient, IBriefUserDto, IUserDto, LikesClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-likes-list',
  templateUrl: './likes-list.component.html',
  styleUrls: ['./likes-list.component.css']
})
export class LikesListComponent implements OnInit {

  users: IBriefUserDto[]= [];
  @Input() postId: number;

  constructor(private router: ActivatedRoute, private likesClient: LikesClient) { }

  ngOnInit() {
    this.likesClient.getLikesOfPost(this.postId).pipe(
      map(likers => likers.map(liker => ({
        ...liker,
        profileImage: liker.profileImage != null ? `api/Images/${liker.profileImage}` : 'api/Images/logoimg.jpg'

      })))
    ).subscribe({
      next: data => {
        this.users = data
      }
    })
  }

}
