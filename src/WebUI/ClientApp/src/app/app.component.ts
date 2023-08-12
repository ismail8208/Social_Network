import { Component } from '@angular/core';
import { LocalService } from './sheard/localService';
import { NotificationServiceService } from './sheard/notification-service.service';
import { ClientNotificationDto } from './sheard/ClientNotificationDto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  content: string;
  Image: string;
  DistId: number;
  showToastFlag = false;
  fromMe: boolean = true;

  title = 'app';
  constructor(private localStore: LocalService, private notificationServiceService: NotificationServiceService) { }

  ngOnInit(): void {
    this.notificationServiceService.ClientNotification.subscribe((clientNot: ClientNotificationDto) => {
      this.content = clientNot.Content;
      this.DistId = clientNot.DistId;
      this.Image = clientNot.Image != null ? this.Image = `api/Images/${clientNot.Image}` : this.Image = 'api/Images/logoimg.jpg';
      if(this.content)
      {
        this.showToastFlag = true;
        setTimeout(() => {
          this.showToastFlag = false;
        }, 2000);
      }
      this.fromMe = this.DistId != parseInt(this.localStore.getData('id'));
    }
    )
  }


}
