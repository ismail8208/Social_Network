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
export class DashboardComponent implements OnInit {
	ngOnInit(): void {
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