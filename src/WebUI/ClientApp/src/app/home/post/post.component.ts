import { Component, OnInit } from '@angular/core';
import { PostDto, PostsClient, LikesClient, CreateLikeCommand, IUserDto, IPostDto } from '../../web-api-client';
import { mergeMap, of } from 'rxjs';
import { Store, select } from '@ngrx/store';
import { selectUser } from 'src/app/stateManagement/user.selectors';
@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {
  public posts: Postsw[] = [];
  showDialog: boolean = true;
  user: IUserDto;
  like = {
    userId: 2,
    postId: 5,
  }
  constructor(private client: PostsClient, private likeClient: LikesClient,private store: Store) { }


  ngOnInit(): void {
    const pageSize = 10;
    let currentPage = 1;

    this.store.pipe(select(selectUser)).subscribe({
      next: (data) => {
        if (data) {
          this.user = data;
        }
      },
    });
    
    this.client.getPostsWithPagination(this.user.id, currentPage, pageSize).subscribe(
      result => {
        this.posts = result.items.map(post =>
          post as Postsw);
        this.fetchLikesForPosts();
        // Listen to scroll events to trigger pagination
        window.addEventListener('scroll', this.onScroll.bind(this));
      }, error => console.error(error));
  }

  fetchLikesForPosts(): void {
    const pageSize = 20; // Number of likes per page
    let currentPage = 1; // Initial page number
    this.posts.forEach(post => {
      this.likeClient.getLikesWithPagination(post.id, currentPage, pageSize).subscribe(likesResult => {

        post.numberOfLikes = likesResult.items.length;
        this.searchLikes(this.user.id, post.id, 1, 10).subscribe(Like => {
          if (Like)
            post.Liked = true;
          else post.Liked = false;
        })}, error => console.error(error));
    });
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
        this.fetchLikesForPosts();
      }, error => console.error(error));
    }
  }

  onLikeClicked(postId: number): void {
    this.checkIfUserLiked(this.user.id, postId).subscribe(
      like => {
        if (like) {

          this.likeClient.delete(like.id).subscribe({
            next: () => {
              this.fetchLikesForPosts();
            },
            error: () => {
              console.error("Failed to remove like");
            }
          });
        } else {

          this.like.postId = postId;
          this.like.userId=this.user.id;
          this.likeClient.create(this.like as CreateLikeCommand).subscribe({
            next: () => {
              this.fetchLikesForPosts();
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
  addComment(display: boolean): void {
    console.log(display)
    this.showDialog = true;
  }
  showdialog(postId: number): void {
    console.log("trig");
    this.showDialog = false;
  }

  openD() {
    const modal = document.querySelector('.modalDialogP') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }

  closeD() {
    const modal = document.querySelector('.modalDialogP') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
  }

  mypost: Postsw;
  send(){
  }


}

export class Postsw extends PostDto {
  public Liked: boolean;
}
