import { Component, Input, OnInit } from '@angular/core';
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
      }
    })
  }
}
