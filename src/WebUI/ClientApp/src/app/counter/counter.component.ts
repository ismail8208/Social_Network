import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { NotificationServiceService } from '../sheard/notification-service.service';
import { ClientNotificationDto } from '../sheard/ClientNotificationDto';
@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html',
  styleUrls: ['counter.component.css']
})
export class CounterComponent implements OnInit{
content: string;
Image: string;
DistId: number;
constructor(private notificationServiceService: NotificationServiceService){

}



  ngOnInit(): void {
    this.notificationServiceService.ClientNotification.subscribe((clientNot: ClientNotificationDto) => {
      this.content = clientNot.Content;
      this.DistId = clientNot.DistId;
      this.Image = clientNot.Image
      this.showToastFlag = true;
      setTimeout(() => {
        this.showToastFlag = false;
      }, 1800);
    }
    )
  }
  showToastFlag = false;
  showToast()
  {
  this.showToastFlag = true;
    setTimeout(() => {
      this.showToastFlag = false;
    }, 1800);
  }
}
