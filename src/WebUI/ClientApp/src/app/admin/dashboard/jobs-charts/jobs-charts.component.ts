import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import Chart from 'chart.js/auto';
import * as moment from 'moment';
import { firstValueFrom } from 'rxjs';
import { DashboardsClient, IJobsInfoDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-jobs-charts',
  templateUrl: './jobs-charts.component.html',
  styleUrls: ['./jobs-charts.component.css']
})
export class JobsChartsComponent implements OnInit {

  public chart: any;
  public chartBar: any;
  jobInfoDto: IJobsInfoDto;
  listLineDateFormated: string[] = [];
  listBarDateFormated: string[] = [];
  DateFrom: Date = new Date('2023-8-5');
  DateTo: Date = new Date('2023-8-14');;
  numOfJobs: number;
  constructor(private dashboardsClient: DashboardsClient) {

  }
  ngOnInit(): void {
    this.createLineChart();
    this.createBarChart();
  }

  update() {
    if (this.DateTo > this.DateFrom) {
    this.listLineDateFormated = []
    this.listBarDateFormated = []
    this.chartBar.destroy()
    this.chart.destroy()
    this.createLineChart();
    this.createBarChart();

    }
  }

  async createLineChart() {

    if (this.DateTo > this.DateFrom) {
      this.jobInfoDto = await firstValueFrom(this.dashboardsClient.getJobCounts(new Date(this.getFormattedDate(this.DateFrom)), new Date(this.getFormattedDate(this.DateTo))));

      this.jobInfoDto.dateTimes.forEach(element => {
        this.listLineDateFormated.push(this.getFormattedDate(element))
      });
      this.numOfJobs = this.jobInfoDto.numberOfAllJobs;
      this.chart = new Chart("MyChart", {
        type: 'line', //this denotes tha type of chart

        data: {// values on X-Axis
          labels: this.listLineDateFormated,
          datasets: [
            {
              label: "jobs",
              data: this.jobInfoDto.numberOfJobs,
              backgroundColor: '#00ADB5'
            },

          ]
        },
        options: {
          aspectRatio: 2.5
        }

      });
    }

  }

  async createBarChart()
  {
    if (this.DateTo > this.DateFrom) {
      this.jobInfoDto = await firstValueFrom(this.dashboardsClient.getJobCounts(new Date(this.getFormattedDate(this.DateFrom)), new Date(this.getFormattedDate(this.DateTo))));

      this.jobInfoDto.dateTimes.forEach(element => {
        this.listBarDateFormated.push(this.getFormattedDate(element))
      });
      this.chartBar = new Chart("MyBarChart", {
        type: 'bar', //this denotes tha type of chart

        data: {// values on X-Axis
          labels: this.listBarDateFormated,
          datasets: [
            {
              label: "jobs",
              data: this.jobInfoDto.numberOfJobs,
              backgroundColor: '#00ADB5'
            },

          ]
        },
        options: {
          aspectRatio: 2.5
        }

      });
    }

  }




  getFormattedDate(created: Date): string {
    const currentDate = moment();
    const jobDate = moment(created);

    return moment(created).format('MMM DD, YYYY');
  }
}
