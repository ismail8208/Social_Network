import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { NotificationServiceService } from '../sheard/notification-service.service';
import { ClientNotificationDto } from '../sheard/ClientNotificationDto';
@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html',
  styleUrls: ['counter.component.css']
})
export class CounterComponent implements OnInit, OnChanges{

   DateFrom: Date;
   DateTo: Date;

constructor(){

}
  ngOnChanges(changes: SimpleChanges): void {
    this.DateFrom;
    this.DateTo;
  }
  ngOnInit(): void {



  }
}
