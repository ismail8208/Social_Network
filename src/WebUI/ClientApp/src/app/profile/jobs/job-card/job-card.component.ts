import { Component, Input, OnInit } from '@angular/core';
import { CV2, ExportCVClient, ICV2, IJobDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-job-card',
  templateUrl: './job-card.component.html',
  styleUrls: ['./job-card.component.css']
})
export class JobCardComponent implements OnInit {

  @Input() job: IJobDto;
  btnText: string ='Send CV';
  constructor(private CvClient: ExportCVClient) { }

  ngOnInit(): void {
  }

  SendCV()
  {
    let cv: ICV2 = {
      company: this.job.userId,
      position: this.job.id,
      userId: parseInt(localStorage.getItem('id'))
    }
    this.CvClient.reveiveCV(cv as CV2).subscribe(
      {
        next: data => {
          if(data>0)
          {
            this.btnText = 'Succeeded'
          }
        }
      }
    )
  }
}
