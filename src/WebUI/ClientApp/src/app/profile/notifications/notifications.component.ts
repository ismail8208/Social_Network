import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { firstValueFrom, map } from 'rxjs';
import { ExportCVClient, INotification, INotificationDto, IUserDto, NotificationsClient, UsersClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {

  notifications: CustomINotification[] = [];
  isAdmin: boolean = false;
  user: IUserDto = {
    firstName: '',
    id: 0,
    numberOfFollowers: 0,
    profileImage: '',
    numberOfFollowings: 0,
    role: '',
    summary: '',
    userName: '',
    numberOfPosts: 0,
  };
  constructor(private clientNotifications: NotificationsClient, private ClientsUsers: UsersClient) { }

  async ngOnInit() {

    this.clientNotifications.getNotifications(parseInt(localStorage.getItem('id'))).pipe(
      map(notifications =>
        notifications.map(
          notify => ({
            ...notify,
            image: notify.image != null ? `api/Images/${notify.image}` : null,
            toWho: this.ifAdmin(notify.content)
          } as CustomINotification)
        ))
    ).subscribe(
      {
        next: data => {
          this.notifications = data.reverse();
        }
      }
    )

    this.user = await firstValueFrom(this.ClientsUsers.get(localStorage.getItem('username')).pipe(
      map(data => ({
        // ...data,
        profileImage: data.profileImage != null ? data.profileImage = `api/Images/${data.profileImage}` : data.profileImage = 'api/Images/logoimg.jpg',
        summary: data.summary != null ? data.summary : '',
        firstName: data.firstName ?? '',
        lastName: data.lastName ?? '',
        id: data.id ?? 0,
        numberOfFollowers: data.numberOfFollowers ?? 0,
        numberOfFollowings: data.numberOfFollowings ?? 0,
        role: data.role ?? '',
        userName: data.userName ?? '',
        specialization: data.specialization ?? '',
        numberOfPosts: data.numberOfPosts ?? 0
      })
      )
    ));
    
  }

  getFormattedDate(created: Date): string {
    const currentDate = moment();
    const postDate = moment(created);
    const diffSecond = currentDate.diff(postDate, 'second');
    const diffMinutes = currentDate.diff(postDate, 'minutes');
    const diffHours = Math.floor(diffMinutes / 60);

    if (diffSecond < 60) {
      return `${diffSecond} second ago`;
    } else if (diffMinutes < 60) {
      return `${diffMinutes} minutes ago`;
    } else if (diffHours < 24) {
      return `${diffHours} hours ago`;
    } else {
      return moment(created).format('MMM DD, YYYY');
    }
  }

  ifAdmin(content: string): string
  {
    const startMarker = '.. ';
    const endMarker = '(';
    const startIndex = content.indexOf(startMarker);
    
    if (startIndex !== -1) {
      const endIndex = content.indexOf(endMarker, startIndex + startMarker.length);
      
      if (endIndex !== -1) {
        return content.substring(startIndex + startMarker.length, endIndex);
      }
    }
    
    return '';
  }
}
export interface CustomINotification extends INotificationDto {
toWho: string;
}