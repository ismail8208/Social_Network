import { Component, OnInit, TemplateRef,  } from '@angular/core';
import { 
  PostDto,
  PostsClient,
  LikesClient, 
  CreateLikeCommand, 
  IUserDto, 
  CommentsClient, 
  CreateCommentCommand, 
  ICommentDto, 
  UsersClient
 } from '../../web-api-client';
import { firstValueFrom, map, mergeMap, of } from 'rxjs';
import { Store, select } from '@ngrx/store';
import { selectUser } from 'src/app/stateManagement/user.selectors';
import * as moment from 'moment';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { LocalService } from 'src/app/sheard/localService';
;
@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {


  newCommentEditor: any = {};
  newCommentModalRef: BsModalRef;

  comments: ICommentDto[];
  thispost: number = 1;
  public posts: Postsw[] = [];
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
  users: IUserDto[];

  like = {
    userId: 2,
    postId: 5,
  }
  constructor(
    private client: PostsClient,
    private likeClient: LikesClient,
    private commentClient: CommentsClient,
    private store: Store,
    private modalService: BsModalService,
    private usersClient:UsersClient,
    private localService: LocalService
  ) { }


  async ngOnInit() {
    const pageSize = 10;
    let currentPage = 1;

    //this.user = await firstValueFrom(this.usersClient.get(this.localService.getData('username')))
    
    this.user = await firstValueFrom(this.usersClient.get(this.localService.getData('username')).pipe(
      map(data => ({
        // ...data,
        profileImage: data.profileImage != null ? data.profileImage = `https://localhost:44447/api/Images/${data.profileImage}` : data.profileImage = 'https://localhost:44447/api/Images/2b388861-8663-4843-9f65-5481388e927d_Screenshot 2023-05-06 211008.png',
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

    // this.store.pipe(select(selectUser)).subscribe({
    //   next: (data) => {
    //     if (data) {
    //       this.user = data;
    //     }
    //   },
    // });

    this.client.getPostsWithPagination(this.user.id, currentPage, pageSize).subscribe(
      result => {
        this.posts = result.items.map(post =>
          post as Postsw);
          this.posts.forEach(post => {
          this.fetchCommentForPosts(post);
        this.fetchLikesForPosts(post);
          });
        // Listen to scroll events to trigger pagination
        window.addEventListener('scroll', this.onScroll.bind(this));
      }, error => console.error(error));

  }


  fetchLikesForPosts(post:Postsw): void {
    const pageSize = 20; // Number of likes per page
    let currentPage = 1; // Initial page number
    let counter;
   
      this.likeClient.getLikesWithPagination(post.id, currentPage, pageSize).subscribe(likesResult => {

        counter = likesResult.items.length;
        while (likesResult.totalPages > currentPage) {
          currentPage++
          this.likeClient.getLikesWithPagination(post.id, currentPage, pageSize).subscribe(likesResult => {

            counter += likesResult.items.length;
          })
        }
        post.numberOfLikes = counter;


        this.searchLikes(this.user.id, post.id, 1, 10).subscribe(Like => {
          if (Like)
            post.Liked = true;
          else post.Liked = false;
        })
      }, error => console.error(error));
    
  }

  onScroll(): void {
    // Calculate the scroll position
    const scrollPosition = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
    // Calculate the height of the page and the height of the viewport
    const pageHeight = document.documentElement.scrollHeight || document.body.scrollHeight || 0;
    const viewportHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight || 0;
    // Calculate the remaining scrollable distance
    const remainingScrollDistance = pageHeight - scrollPosition - viewportHeight;
    // Define the threshold for triggering pagination (e.g., when 80% of the page is scrolled)
    const paginationThreshold = 0.8;
    // Check if the remaining scrollable distance meets the pagination threshold
    if (remainingScrollDistance / pageHeight <= (1 - paginationThreshold)) {
      // Increment the current page number
      const pageSize = 10; // Number of posts per page
      let currentPage = Math.ceil(this.posts.length / pageSize) + 1;
      // Fetch the next page of posts
      this.client.getPostsWithPagination(this.user.id, currentPage, pageSize).subscribe(result => {
        // Append the new posts to the existing posts array
        this.posts = this.posts.concat(
          result.items.map(post => post as Postsw)
        );
        // Fetch likes for the new posts
        this.posts.forEach(post => {
          
          this.fetchCommentForPosts(post);
          this.fetchLikesForPosts(post);
            });
      }, error => console.error(error));
    }
  }

  onLikeClicked(postId: number): void {
    this.checkIfUserLiked(this.user.id, postId).subscribe(
      like => {
        if (like) {

          this.likeClient.delete(like.id).subscribe({
            next: () => {
              this.fetchLikesForPosts(this.posts.find(post => post.id==postId));
            },
            error: () => {
              console.error("Failed to remove like");
            }
          });
        } else {

          this.like.postId = postId;
          this.like.userId = this.user.id;
          this.likeClient.create(this.like as CreateLikeCommand).subscribe({
            next: () => {
              this.fetchLikesForPosts(this.posts.find(post => post.id==postId));
            },
            error: () => {
              console.error("Failed to add like");
            }
          });
        }
      },
      error => console.error(error)
    );
  }

  checkIfUserLiked(userId: number, postId: number): any {
    const pageSize = 10; // Number of likes per page
    let currentPage = 1; // Initial page number
    return this.searchLikes(userId, postId, currentPage, pageSize);
  }

  searchLikes(userId: number, postId: number, currentPage: number, pageSize: number): any {
    return this.likeClient.getLikesWithPagination(postId, currentPage, pageSize).pipe(
      mergeMap(result => {
        const like = result.items.find(Like => Like.userId === userId);
        if (like) {
          return of(like); // Like found, return it
        } else if (result.totalPages > currentPage) {
          // More pages available, search in the next page
          currentPage++;
          return this.searchLikes(userId, postId, currentPage, pageSize);
        } else {
          return of(null); // Like not found, return null
        }
      })
    );
  }

  getFormattedDate(created: Date): string {
    const currentDate = moment();
    const postDate = moment(created);
    const diffMinutes = currentDate.diff(postDate, 'minutes');
    const diffHours = Math.floor(diffMinutes / 60);

    if (diffMinutes < 60) {
      return `${diffMinutes} minutes ago`;
    } else if (diffHours < 24) {
      return `${diffHours} hours ago`;
    } else {
      return moment(created).format('MMM DD, YYYY');
    }
  }





  fetchCommentForPosts(post: Postsw): void {
    let currentPage = 1;
    let counter;
    this.commentClient.getCommentsWithPagination(post.id, currentPage, 20).subscribe(
      
       data => {
          counter = data.items.length;
          this.comments = data.items;
          while (data.totalPages > currentPage) {
            currentPage++
            this.commentClient.getCommentsWithPagination(post.id, currentPage, 20).subscribe(data => {
              counter += data.items.length;

              this.comments.push(data.items as ICommentDto)
            })
          }
          post.numberOfComments = counter;

        }
      
    );
    

  }

  showNewCommentModal(template: TemplateRef<any>, postId: number): void {
    this.thispost = postId;
    this.newCommentModalRef = this.modalService.show(template);

    this.fetchCommentForPosts(this.posts.find(post => post.id==postId));
    setTimeout(() => document.getElementById('content').focus(), 250);
  }


  newCommentCancelled(): void {
    this.newCommentModalRef.hide();
    this.newCommentEditor = {};
  }

  addComment(): void {
    if (this.newCommentEditor.content != "") {
      const comment = {
        content: this.newCommentEditor.content,
        userId: this.user.id,
        postId: this.thispost

      } as CreateCommentCommand;
      
      this.commentClient.create(comment).subscribe(  error => console.error(error))
      this.newCommentEditor.content='';
      this.fetchCommentForPosts(this.posts.find(post => post.id==this.thispost));
    }
  

  }

}

export class Postsw extends PostDto {
  public Liked: boolean;
}