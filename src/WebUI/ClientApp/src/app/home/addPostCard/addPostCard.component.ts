
import { Component, OnInit, TemplateRef } from '@angular/core';
import {FileParameter, IUserDto, PostsClient, UsersClient} from '../../web-api-client';
import { Store, select } from '@ngrx/store';
import { selectUser } from 'src/app/stateManagement/user.selectors';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { LocalService } from 'src/app/sheard/localService';
import { firstValueFrom, map } from 'rxjs';

@Component({
  selector: 'app-addPostCard',
  templateUrl: './addPostCard.component.html',
  styleUrls: ['./addPostCard.component.css']
})

export class AddPostCardComponent implements OnInit {
  debug = false;
  newPostEditor: any = {};
  newPost: INewPost = {
    content:'',
    image: undefined,
    video: undefined
  };
  newPostModalRef: BsModalRef;
  user: IUserDto = {
    firstName: '',
    lastName: '',
    id: 0,
    numberOfFollowers: 0,
    profileImage: '',
    numberOfFollowings: 0,
    role: '',
    summary: '',
    userName: '',
  };
  constructor(private store: Store,    private usersClient:UsersClient,    private modalService: BsModalService ,private postsClinte: PostsClient,private localService: LocalService){}
  async ngOnInit() {
    
    this.user = await firstValueFrom(this.usersClient.get(this.localService.getData('username')).pipe(
      map(data => ({
        // ...data,
        profileImage: data.profileImage != null ? data.profileImage = `api/Images/${data.profileImage}` : data.profileImage = 'api/Images/logoimg.jpg',
        summary: data.summary != null ? data.summary : '',
        firstName: data.firstName ?? '',
        lastName: data.lastName ?? '',
        id: data.id ?? 0,
        numberOfFollowers: data.numberOfFollowers ?? 0,
        numberOfFollowings: data.numberOfFollowings ?? 0,
        role: data.role ?? '',
        userName: data.userName ?? ''

      })
      )
    ))
  }
  
  showNewPostModal(template: TemplateRef<any>): void {

    this.newPostModalRef = this.modalService.show(template);

    setTimeout(() => {document.getElementById('content').focus(), 250;
    document.getElementById('image').focus(), 250;
    document.getElementById('video').focus(), 250;
  
    });
  }
  newPostCancelled(): void {
    this.newPostModalRef.hide();
    this.newPostEditor = {};
  }
  addPost(): void {
    
      this.postsClinte.create(this.newPost.content,this.newPost.image,this.newPost.video,this.user.id)
      .subscribe( error => console.error(error))
      this.newPost.content= '' ;
      this.newPostCancelled();
    
  }
}

export interface INewPost {
  content: string | null | undefined,
  image: FileParameter | null | undefined,
  video: FileParameter | null | undefined,
}