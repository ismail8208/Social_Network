import { ThisReceiver } from '@angular/compiler';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { INewPost } from 'src/app/home/addPostCard/addPostCard.component';
import { FileParameter, IUserDto, PostsClient, UsersClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-write-post',
  templateUrl: './write-post.component.html',
  styleUrls: ['./write-post.component.css']
})
export class WritePostComponent implements OnInit {
  @Input() user: IUserDto;

  btnText: string = 'Post'
  @Output() postSucceeded: EventEmitter<true> = new EventEmitter<true>();

  newPost: CreatePostCommand = {
    content: '',
    image: undefined,
    video: undefined,

  };

  imageFile: any;


  formData: FormData = new FormData();


  constructor(private postsClient: PostsClient) { }


  async ngOnInit() {
  }
  eventl(event: any){
    const selectedFile: File = event.target.files[0];
    this.imageFile=selectedFile;
  }
  cancele(){
    this.postSucceeded.emit(true);
  }
  CreatePost() {
  
    var imageForPost = {
      data: this.imageFile,
      fileName:  this.imageFile.name
    } as FileParameter;
    
    // var IImage = {
    //   data: this.newPost.image.target.files[0],
    //   name: this.newPost.image.name

    // } as FileParameter
    //IImage = this.imageFile.target.files[0];



    this.postsClient.create(this.newPost.content, imageForPost, this.newPost.video, this.user.id).subscribe(
      {
        next: data => {
          if (data > 0) {
            this.btnText = 'succeeded';
            this.newPost.content = '';
            this.newPost.image = undefined;
            this.newPost.video = undefined;
            this.postSucceeded.emit(true);
          }
        }
      }
    )
  }


}

export interface CreatePostCommand {
  content: string | null | undefined,
  image: any,
  video: any,
}
