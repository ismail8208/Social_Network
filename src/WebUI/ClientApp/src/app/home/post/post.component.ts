import { Component, OnDestroy, OnInit, TemplateRef, } from '@angular/core';
import {
  PostDto,
  PostsClient,
  LikesClient,
  CreateLikeCommand,
  IUserDto,
  CommentsClient,
  CreateCommentCommand,
  UsersClient,
  CommentDto,
  NotificationsClient,

} from '../../web-api-client';
import { Subscription, firstValueFrom, map, mergeMap, of } from 'rxjs';
import * as moment from 'moment';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { LocalService } from 'src/app/sheard/localService';


@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit , OnDestroy{
 
  isOwner: boolean = false;
  // Add a variable to track if a request is in progress
  private isFetchingPosts = false;
  

  newCommentEditor: any = {};
  newCommentModalRef: BsModalRef;
  LikesModalRef: BsModalRef;

  deletePostModalRef: BsModalRef;
  postIdForDelete: number;
  deleteCommentModalRef: BsModalRef
  commentIdForDelete: number;

  editePostModalRef: BsModalRef;
  postIdForedite: PostDto;


  comments: CommentForView[];
  commentsX: CommentForView[];


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
    specialization:'',
  };
  users: IUserDto[];2

  like = {
    userId: 2,
    postId: 5,
  }


  posts$ : Subscription;
  posts1$ : Subscription;

  
  constructor(
    private client: PostsClient,
    private likeClient: LikesClient,
    private commentClient: CommentsClient,
    private modalService: BsModalService,
    private usersClient: UsersClient,
    private localService: LocalService,
    private notificationsClient: NotificationsClient
  ) { }


  
  ngOnDestroy(): void {
    // this.posts$.unsubscribe(); 
    // this.posts1$.unsubscribe(); 
      
   

  }


  async ngOnInit() {
    const pageSize = 10;
    let currentPage = 1;

    //this.user = await firstValueFrom(this.usersClient.get(this.localService.getData('username')))

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
        userName: data.userName ?? '',
        specialization: data.specialization ?? '',
       

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

       this.posts$= this.client.latestNews(this.user.id, currentPage, pageSize).subscribe(
      result => {
        this.posts = result.items.map(post =>

          post as Postsw


        );
        this.posts.forEach(post => {
          this.usersClient.get(post.userName).subscribe(user => {
            post.firstName = user.firstName;
            post.lastName = user.lastName;
            post.profileImage= user.profileImage != null ? post.profileImage = `api/Images/${user.profileImage}` : post.profileImage = 'api/Images/logoimg.jpg',
            post.specialization = user.specialization;
            post.isOwner=this.localService.getData('username') === post.userName;

          });
          this.fetchCommentForPosts(post);
          this.fetchLikesForPosts(post);
        });
        // Listen to scroll events to trigger pagination
        window.addEventListener('scroll', this.onScroll.bind(this));
      }, error => console.error(error));

  }


  fetchLikesForPosts(post: Postsw): void {
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
    const scrollPosition = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;

    const pageHeight = document.documentElement.scrollHeight || document.body.scrollHeight || 0;
    const viewportHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight || 0;
    const remainingScrollDistance = pageHeight - scrollPosition - viewportHeight;
    const paginationThreshold = 0.8;
    if (!this.isFetchingPosts && remainingScrollDistance / pageHeight <= (1 - paginationThreshold)) {
      this.isFetchingPosts = true;

      const pageSize = 10;
      let currentPage = Math.ceil(this.posts.length / pageSize) + 1;

      this.posts1$=this.client.latestNews(this.user.id, currentPage, pageSize).subscribe(result => {
        this.posts = this.posts.concat(result.items.map(post => post as Postsw));

        this.posts.forEach(post => {
          this.usersClient.get(post.userName).subscribe(user => {
            post.firstName = user.firstName;
            post.lastName = user.lastName;
            post.profileImage= user.profileImage != null ? post.profileImage = `api/Images/${user.profileImage}` : post.profileImage = 'api/Images/logoimg.jpg',
            post.specialization = user.specialization;
            post.isOwner=this.localService.getData('username') === post.userName;

          });
          this.fetchCommentForPosts(post);
          this.fetchLikesForPosts(post);
        });

        this.isFetchingPosts = false;
      }, error => {
        this.isFetchingPosts = false;
        console.error(error);
      });
    }
  }

  
  reportBtn: string = 'Report'
  Report(id: number)
  {
    console.log(`${this.user.id} id of absou`)

    this.notificationsClient.report(this.user.id, id).subscribe({
      next: data => {
        if(data)
        {
          this.reportBtn = 'Reported'
        }
      }
    })
  }

  getLikes(template: TemplateRef<any>, postId: number): void {
    this.thispost = postId;
    this.LikesModalRef = this.modalService.show(template);
  }
  
  onLikeClicked(postId: number): void {
    this.checkIfUserLiked(this.user.id, postId).subscribe(
      like => {
        if (like) {

          this.likeClient.delete(like.id).subscribe({
            next: () => {
              this.fetchLikesForPosts(this.posts.find(post => post.id == postId));
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
              this.fetchLikesForPosts(this.posts.find(post => post.id == postId));
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
    const diffSecond = currentDate.diff(postDate, 'second');
    const diffMinutes = currentDate.diff(postDate, 'minutes');
        const diffHours = Math.floor(diffMinutes / 60);

     if (diffSecond < 60) {
      return `${diffSecond} second ago`;
    } else if (diffMinutes < 60) {
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
    this.commentClient.getCommentsWithPagination(post.id, currentPage,100).subscribe(

      data => {
      
        counter = data.items.length;
        this.comments = data.items.map(comment =>

          comment as CommentForView
        );
        
        this.comments.forEach(comment => {
          this.usersClient.get(comment.userName).subscribe(user => {
            comment.firstName = user.firstName;
            comment.lastName = user.lastName;
            comment.profileImage= user.profileImage != null ? comment.profileImage = `api/Images/${user.profileImage}` : comment.profileImage = 'api/Images/logoimg.jpg',
            comment.specialization = user.specialization;
            comment.isOwner=this.localService.getData('username') === comment.userName;
            comment.postOwner=this.user.userName === comment.userName;
          });
        });
     
        while (data.totalPages > currentPage) {
          currentPage++;
          this.commentClient.getCommentsWithPagination(post.id, currentPage, 20).subscribe(data => {
            counter += data.items.length;
            this.commentsX = data.items.map(comment =>

              comment as CommentForView
            );
            
            this.commentsX.forEach(comment => {
              this.usersClient.get(comment.userName).subscribe(user => {
                console.log(data.totalPages +"in while" );
                comment.firstName = user.firstName;
                comment.lastName = user.lastName;
                comment.profileImage = user.profileImage;
                comment.specialization = user.specialization;
                comment.isOwner=this.localService.getData('username') === comment.userName;
                comment.postOwner=this.user.userName === comment.userName;
              });
            });

            this.comments.push(...this.commentsX)
          })
        }
        post.numberOfComments = counter;

      }

    );


  }

  showNewCommentModal(template: TemplateRef<any>, postId: number): void {
    this.thispost = postId;
    this.newCommentModalRef = this.modalService.show(template);

    this.fetchCommentForPosts(this.posts.find(post => post.id == postId));
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

      this.commentClient.create(comment).subscribe(error => console.error(error))
      this.newCommentEditor.content = '';
      this.fetchCommentForPosts(this.posts.find(post => post.id == this.thispost));
    }


  }



  showDeletePostModal(template: TemplateRef<any>, postId: number): void {
    this.deletePostModalRef = this.modalService.show(template);
    this.postIdForDelete = postId;
  }

  deletePostCancelled(): void {
    this.deletePostModalRef.hide();
  }

  deletePost(): void {
    this.client.delete(this.postIdForDelete).subscribe(error => console.error(error))

    this.deletePostCancelled();
  }

  




  showEditePostModal(template: TemplateRef<any>, post: PostDto): void {
    this.editePostModalRef = this.modalService.show(template);
    this.postIdForedite = post;
  }

  editePostCancelled(): void {
    this.editePostModalRef.hide();
  }

  editePost(): void {
    this.client.update(this.postIdForedite.id,this.postIdForedite.content).subscribe(error => console.error(error))

    this.editePostCancelled();
  }



  
  showDeleteCommentModal(template: TemplateRef<any>, commentId: number): void {
    this.deleteCommentModalRef = this.modalService.show(template);
    this.commentIdForDelete = commentId;
  }

  deleteCommentCancelled(): void {
    this.deleteCommentModalRef.hide();
  }

  deleteComment(): void {
    this.commentClient.delete(this.commentIdForDelete).subscribe(error => console.error(error));

    this.deleteCommentCancelled();
  }





  downloadImage(imageUrl: string): void {
    const link = document.createElement('a');
    link.href = "api/Images/" + imageUrl;
    link.download = "api/Images/" + imageUrl; // The default downloaded file name
    link.target = '_blank';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  copyTextToClipboard(textToCopy:string): void {

    const tempInput = document.createElement('input');
    tempInput.value = textToCopy;
    document.body.appendChild(tempInput);
    tempInput.select();
    document.execCommand('copy');
    document.body.removeChild(tempInput);
    document.getElementById("copy").textContent="copied";
  }
}

export class Postsw extends PostDto {
  public Liked: boolean;
  firstName?: string | undefined;
  lastName?: string | undefined;
  profileImage?: string | undefined;
  specialization?: string | undefined;
  isOwner: boolean;
}

export class CommentForView extends CommentDto{
  firstName?: string | undefined;
  lastName?: string | undefined;
  profileImage?: string | undefined;
  specialization?: string | undefined;
  postOwner: boolean;
  isOwner: boolean;
}
