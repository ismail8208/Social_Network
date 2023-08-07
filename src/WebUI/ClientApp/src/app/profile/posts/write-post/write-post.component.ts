import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom, map } from 'rxjs';
import { FileParameter, IUserDto, PostsClient, UsersClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-write-post',
  templateUrl: './write-post.component.html',
  styleUrls: ['./write-post.component.css']
})
export class WritePostComponent implements OnInit {
  @Input() user: IUserDto;
  content: string;
  btnText: string = 'Post'
  @Output() postSucceeded: EventEmitter<true> = new EventEmitter<true>();
  constructor(private postsClient: PostsClient) { }


  async ngOnInit() {
  }

  CreatePost()
  {
    this.postsClient.create(this.content, undefined, undefined, this.user.id).subscribe(
      {
        next: data => {
          if(data>0)
          {
            this.btnText = 'succeeded';
            this.content = '';
            this.postSucceeded.emit(true);
          }
        }
      }
    )
  }


}

export interface CreatePostCommand
{
  content: string | null | undefined,
  image: FileParameter | null | undefined,
  video: FileParameter | null | undefined,
  userId: number | undefined
}
