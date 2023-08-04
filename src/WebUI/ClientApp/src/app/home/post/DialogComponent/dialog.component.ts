import { Component, Output,EventEmitter, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CommentsClient, CreateCommentCommand, ICommentDto, ICreateCommentCommand, IPostDto } from 'src/app/web-api-client';
import { Postsw } from '../post.component';
import { Store, select } from '@ngrx/store';
import { selectUser } from 'src/app/stateManagement/user.selectors';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit{

  comments: ICommentDto[];
  comment: string;
  userId: number;
  constructor(private commentClient: CommentsClient, private store: Store)
  {
  }
  ngOnInit(): void {
    
    this.store.pipe(select(selectUser)).subscribe({
      next: (data) => {
        if (data) {
          this.userId = data.id;
        }
      },
    });
    this.commentClient.getCommentsWithPagination(this.post.id, 1, 10).subscribe(
      {
        next: data => this.comments = data.items
      }
    );
  }

  @Input() post?: Postsw;
  @Output() chosenPost: EventEmitter<IPostDto> = new EventEmitter<IPostDto>();

  closeD(){
    console.log("from close D ")
    const modal = document.querySelector('.modalDialogP') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
  }

  openD() {
    const modal = document.querySelector('.modalDialogP') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }

  addComment()
  {
    let entity: ICreateCommentCommand = {
      postId: this.post.id,
      userId: this.userId,
      content: this.comment
    };

    this.commentClient.create(entity as CreateCommentCommand).subscribe();

    this.commentClient.getCommentsWithPagination(this.post.id, 1, 50).subscribe(
      {
        next: data => this.comments = data.items
      }
    );
  }
}