import { Component, Input, OnInit } from '@angular/core';
import * as moment from 'moment';
import { IPost, IPostDto, PostsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-brief-posts',
  templateUrl: './brief-posts.component.html',
  styleUrls: ['./brief-posts.component.css']
})
export class BriefPostsComponent implements OnInit {

  posts: IPostDto[] = []
  @Input() userId: number;
  constructor(private postsClient: PostsClient) { }

  ngOnInit(): void {

    this.postsClient.getPostsWithPagination(this.userId, 1,3).subscribe({
      next: data => {
        this.posts = data.items;
        this.posts.forEach( post=>{
          post.imageURL=(!post.imageURL && !post.videoURL)?'logoimg.jpg':post.imageURL;
          post.videoURL=(!post.imageURL && post.videoURL)?'logoimg.jpg':post.videoURL;
        }

        )
      }
    })

  }

   getFormattedDate(created: Date): string {
    const currentDate = moment();
    const postDate = moment(created);

      return moment(created).format('MMM DD, YYYY');
    }
}
