import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import Chart from 'chart.js/auto';
import * as moment from 'moment';
import { firstValueFrom } from 'rxjs';
import { DashboardsClient, IPostInfoDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-posts-charts',
  templateUrl: './posts-charts.component.html',
  styleUrls: ['./posts-charts.component.css']
})
export class PostsChartsComponent implements OnInit {

  public chart: any;
  public chartBar: any;
  postInfoDto: IPostInfoDto;
  listLineDateFormated: string[] = [];
  listBarDateFormated: string[] = [];
  DateFrom: Date = new Date('2023-8-5');
  DateTo: Date = new Date('2023-8-14');;
  numOfPosts: number;
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
      this.postInfoDto = await firstValueFrom(this.dashboardsClient.getPostCounts(new Date(this.getFormattedDate(this.DateFrom)), new Date(this.getFormattedDate(this.DateTo))));

      this.postInfoDto.dateTimes.forEach(element => {
        this.listLineDateFormated.push(this.getFormattedDate(element))
      });
      this.numOfPosts = this.postInfoDto.numberOfAllPosts;
      this.chart = new Chart("MyChart", {
        type: 'line', //this denotes tha type of chart

        data: {// values on X-Axis
          labels: this.listLineDateFormated,
          datasets: [
            {
              label: "posts",
              data: this.postInfoDto.numberOfPosts,
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
      this.postInfoDto = await firstValueFrom(this.dashboardsClient.getPostCounts(new Date(this.getFormattedDate(this.DateFrom)), new Date(this.getFormattedDate(this.DateTo))));

      this.postInfoDto.dateTimes.forEach(element => {
        this.listBarDateFormated.push(this.getFormattedDate(element))
      });
      this.chartBar = new Chart("MyBarChart", {
        type: 'bar', //this denotes tha type of chart

        data: {// values on X-Axis
          labels: this.listBarDateFormated,
          datasets: [
            {
              label: "posts",
              data: this.postInfoDto.numberOfPosts,
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
    const postDate = moment(created);

    return moment(created).format('MMM DD, YYYY');
  }
}
