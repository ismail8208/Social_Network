import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import Chart from 'chart.js/auto';
import * as moment from 'moment';
import { firstValueFrom } from 'rxjs';
import { DashboardsClient, IUserInfoDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit,OnChanges {
  public chart: any;
  userInfoDto: IUserInfoDto;
  listDateFormated: string[] = [];
  @Input() DateFrom: Date = new Date('2023-8-5');
  @Input() DateTo: Date =  new Date('2023-8-14');;
  constructor(private dashboardsClient: DashboardsClient)
  {

  }
	ngOnChanges(changes: SimpleChanges): void {
		this.createChart();
	}
  ngOnInit(): void {
	this.chart.destroy()
    this.createChart();
  }
  
   async createChart(){
	console.log(this.DateFrom)
	console.log(this.DateTo)
	this.userInfoDto = await firstValueFrom(this.dashboardsClient.getUserCounts(new Date(this.getFormattedDate(this.DateFrom)),new Date(this.getFormattedDate(this.DateTo))));

	this.userInfoDto.dateTimes.forEach(element => {
		this.listDateFormated.push(this.getFormattedDate(element))
	});
    this.chart = new Chart("MyChart", {
      type: 'bar', //this denotes tha type of chart

      data: {// values on X-Axis
        labels: this.listDateFormated, 
	       datasets: [
          {
            label: "Sales",
            data: this.userInfoDto.numberOfUsers,
            backgroundColor: '#00ADB5'
          },
 
        ]
      },
      options: {
        aspectRatio:2.5
      }
      
    });

  }

  getFormattedDate(created: Date): string {
    const currentDate = moment();
    const postDate = moment(created);

      return moment(created).format('MMM DD, YYYY');
    }


}


// this.chart = new Chart("MyChart", {
// 	type: 'bar', //this denotes tha type of chart

// 	data: {// values on X-Axis
// 	  labels: ['2022-05-10', '2022-05-11', '2022-05-12','2022-05-13',
// 							   '2022-05-14', '2022-05-15', '2022-05-16','2022-05-17', ], 
// 		 datasets: [
// 		{
// 		  label: "Sales",
// 		  data: ['467','576', '572', '79', '92',
// 							   '574', '573', '576'],
// 		  backgroundColor: '#00ADB5'
// 		},

// 	  ]
// 	},
// 	options: {
// 	  aspectRatio:2.5
// 	}
	
//   });
// }