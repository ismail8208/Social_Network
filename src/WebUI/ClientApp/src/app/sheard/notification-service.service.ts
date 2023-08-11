import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from '@microsoft/signalr';
import { BehaviorSubject, Observable, firstValueFrom } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { ClientNotificationDto } from './ClientNotificationDto';

@Injectable({
  providedIn: 'root'
})
export class NotificationServiceService {
  // hubUrl: string;
  // connection: any;
  // // private notificationsSubject: Subject<string> = new Subject<string>();
  // constructor(public authorize: AuthorizeService) {    
  //   this.hubUrl = 'Hubs/Notification';
  // }

  // public async initiateSignalrConnection(): Promise<void> {
  //   try {

  //     var isAuthinticated = await firstValueFrom(this.authorize.isAuthenticated());

  //     if (isAuthinticated) {

  //       var token = await firstValueFrom(this.authorize.getAccessToken());

  //       this.connection = new signalR.HubConnectionBuilder()
  //         .withUrl(this.hubUrl, { withCredentials: true, accessTokenFactory: () => token })
  //         .withAutomaticReconnect()
  //         .build();

  //       await this.connection.start({ withCredentials: true });
        
  //       // this.connection.on('ReceiveFollowNotification', (message: string) => {
  //       //   this.notificationsSubject.next(message);
  //       // });

  //       console.log(`SignalR connection success! connectionId: ${this.connection.connectionId}`);

  //     }
      
  //   }
  //   catch (error) {
  //     console.log(`SignalR connection error: ${error}`);
  //   }
  // }

  // public setSignalrClientMethods(): void {
  //   this.connection.on('updateTotalViews', () => {
  //     console.log(`from signalR`);
  //   });

  // }




   hubUrl: string;
  connection: any;
  ClientNotification: BehaviorSubject<ClientNotificationDto>;

  constructor(public authorize: AuthorizeService) {
    var n = new ClientNotificationDto();
    n.Content = "";

    this.hubUrl = 'Hubs/Notification';
    this.ClientNotification = new BehaviorSubject<ClientNotificationDto>(n);
  }

  public async initiateSignalrConnection(): Promise<void> {
    try {

      var isAuthinticated = await firstValueFrom(this.authorize.isAuthenticated());

      if (isAuthinticated) {

        var token = await firstValueFrom(this.authorize.getAccessToken());

        this.connection = new signalR.HubConnectionBuilder()
          .withUrl(this.hubUrl, { withCredentials: true, accessTokenFactory: () => token })
          .withAutomaticReconnect()
          .build();

        await this.connection.start({ withCredentials: true });

        this.setSignalrClientMethods();

        console.log(`SignalR connection success! connectionId: ${this.connection.connectionId}`);

      }

    }
    catch (error) {
      console.log(`SignalR connection error: ${error}`);
    }
  }

  private setSignalrClientMethods(): void {
    this.connection.on('MyNotification', (notification: ClientNotificationDto) => {
      this.ClientNotification.next(notification);
    });

  }
}

  // hubUrl: string;
  // connection: any;
  // ClientNotification: BehaviorSubject<ClientNotificationDto>;

  // constructor(public authorize: AuthorizeService) {
  //   var n = new ClientNotificationDto();
  //   n.Message = "";

  //   this.hubUrl = 'Hubs/Notification';
  //   this.ClientNotification = new BehaviorSubject<ClientNotificationDto>(n);
  // }

  // public async initiateSignalrConnection(): Promise<void> {
  //   try {

  //     var isAuthinticated = await firstValueFrom(this.authorize.isAuthenticated());

  //     if (isAuthinticated) {

  //       var token = await firstValueFrom(this.authorize.getAccessToken());

  //       this.connection = new signalR.HubConnectionBuilder()
  //         .withUrl(this.hubUrl, { withCredentials: true, accessTokenFactory: () => token })
  //         .withAutomaticReconnect()
  //         .build();

  //       await this.connection.start({ withCredentials: true });

  //       this.setSignalrClientMethods();

  //       console.log(`SignalR connection success! connectionId: ${this.connection.connectionId}`);

  //     }

  //   }
  //   catch (error) {
  //     console.log(`SignalR connection error: ${error}`);
  //   }
  // }

  // private setSignalrClientMethods(): void {
  //   this.connection.on('GetNotification', (notification: ClientNotificationDto) => {
  //     this.ClientNotification.next(notification);
  //   });

  // }
