import { Component, Input, OnInit } from '@angular/core';
import { firstValueFrom, map, tap } from 'rxjs';
import { IJobDto, JobsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-jobs-list',
  templateUrl: './jobs-list.component.html',
  styleUrls: ['./jobs-list.component.css']
})
export class JobsListComponent implements OnInit {

  @Input() userId: number;
  jobs: IJobDto[] = [];
  @Input() isOwner: boolean;
  constructor(private jobsClient: JobsClient) { }

  async ngOnInit() {
   this.jobsClient.getJobsWithPagination(this.userId,1,20).subscribe({
      next: data => {this.jobs = data.items; }
    })
    // this.jobs = await firstValueFrom(this.jobsClient.getJobsWithPagination(this.userId, 1, 20).pipe(
    //   map(data => data.items),
    // ))

  }

}
