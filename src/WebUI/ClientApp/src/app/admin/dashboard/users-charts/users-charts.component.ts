import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import Chart from 'chart.js/auto';
import * as moment from 'moment';
import { firstValueFrom } from 'rxjs';
import { DashboardsClient, IUserInfoDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-users-charts',
  templateUrl: './users-charts.component.html',
  styleUrls: ['./users-charts.component.css']
})
export class UsersChartsComponent implements OnInit {

  public chart: any;
  public chartBar: any;
  userInfoDto: IUserInfoDto;
  listLineDateFormated: string[] = [];
  listBarDateFormated: string[] = [];
  DateFrom: Date = new Date('2023-8-5');
  DateTo: Date = new Date('2023-8-14');;
  numOfUsers: number;
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
      this.userInfoDto = await firstValueFrom(this.dashboardsClient.getUserCounts(new Date(this.getFormattedDate(this.DateFrom)), new Date(this.getFormattedDate(this.DateTo))));

      this.userInfoDto.dateTimes.forEach(element => {
        this.listLineDateFormated.push(this.getFormattedDate(element))
      });
      this.numOfUsers = this.userInfoDto.numberOfAllUsers;
      this.chart = new Chart("MyChart", {
        type: 'line', //this denotes tha type of chart

        data: {// values on X-Axis
          labels: this.listLineDateFormated,
          datasets: [
            {
              label: "Users",
              data: this.userInfoDto.numberOfUsers,
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
      this.userInfoDto = await firstValueFrom(this.dashboardsClient.getUserCounts(new Date(this.getFormattedDate(this.DateFrom)), new Date(this.getFormattedDate(this.DateTo))));

      this.userInfoDto.dateTimes.forEach(element => {
        this.listBarDateFormated.push(this.getFormattedDate(element))
      });
      this.chartBar = new Chart("MyBarChart", {
        type: 'bar', //this denotes tha type of chart

        data: {// values on X-Axis
          labels: this.listBarDateFormated,
          datasets: [
            {
              label: "Users",
              data: this.userInfoDto.numberOfUsers,
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
