import { Component, Input, OnInit } from '@angular/core';
import { IJobDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-job-card',
  templateUrl: './job-card.component.html',
  styleUrls: ['./job-card.component.css']
})
export class JobCardComponent implements OnInit {

  @Input() job: IJobDto;
  constructor() { }

  ngOnInit(): void {
  }

}
