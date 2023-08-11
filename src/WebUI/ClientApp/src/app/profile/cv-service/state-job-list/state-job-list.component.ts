import { Component, OnInit } from '@angular/core';
import { IJobDto, JobsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-state-job-list',
  templateUrl: './state-job-list.component.html',
  styleUrls: ['./state-job-list.component.css']
})
export class StateJobListComponent implements OnInit {

  userId: number;
  jobs: IJobDto[] = [];
  lenOfJob: number;
  constructor(private jobsClient: JobsClient) { }

  ngOnInit(): void {
    this.userId = parseInt(localStorage.getItem('id'));

    this.jobsClient.getJobsWithPagination(this.userId, 1, 100).subscribe(
      {
        next: data => {
          this.jobs = data.items;
          this.lenOfJob = data.totalCount;
        }
      }
    )

  }

}
