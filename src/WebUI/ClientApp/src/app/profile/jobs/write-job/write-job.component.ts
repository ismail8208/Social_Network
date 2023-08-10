import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CreateJobCommand, ICreateJobCommand, IUserDto, JobsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-write-job',
  templateUrl: './write-job.component.html',
  styleUrls: ['./write-job.component.css']
})
export class WriteJobComponent implements OnInit {

  @Input() user: IUserDto;
  title: string;
  description: string;
  btnText: string = 'Post'
  @Output() jobSucceeded: EventEmitter<true> = new EventEmitter<true>();
  constructor(private jobsClient: JobsClient) { }


  async ngOnInit() {
  }

  CreatePost() {
    let entity: ICreateJobCommand =
    {
      title: this.title,
      description: this.description,
      userId: this.user.id
    }
    if (this.title.length > 1 && this.description.length > 20) {
      this.jobsClient.create(entity as CreateJobCommand).subscribe(
        {
          next: data => {
            if (data > 0) {
              this.btnText = 'succeeded';
              this.description = '';
              this.title = '';
              this.jobSucceeded.emit(true);
            }
          }
        }
      )
    }

  }

}
