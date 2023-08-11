import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CV, ExportCVClient, IUserDto, JobsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-cv-list',
  templateUrl: './cv-list.component.html',
  styleUrls: ['./cv-list.component.css']
})
export class CvListComponent implements OnInit {

  jobId: number;
  users: IUserDto[] = [];
  constructor(private exportCVClient: ExportCVClient, private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.jobId =parseInt(this.router.snapshot.paramMap.get('jobId'));
    this.exportCVClient.getCVs(this.jobId).subscribe(
      {
        next: data => this.users = data
      }
    )

  }

}
