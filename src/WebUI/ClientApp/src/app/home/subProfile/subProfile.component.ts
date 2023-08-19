
import { Component, OnInit, TemplateRef } from '@angular/core';
import { firstValueFrom, map } from 'rxjs';
import { LocalService } from 'src/app/sheard/localService';
import { AddressesClient, IUserDto, UsersClient } from 'src/app/web-api-client';


@Component({
  selector: 'app-subProfile',
  templateUrl: './subProfile.component.html',
  styleUrls: ['./subProfile.component.css']
})

export class SubProfileComponent implements OnInit {
 
  
  user: IUserForSummary = {
    firstName: '',
    lastName: '',
    id: 0,
    numberOfFollowers: 0,
    profileImage: '',
    numberOfFollowings: 0,
    role: '',
    summary: '',
    userName: '',
    numberOfPosts: 0,
    address: '',
    specialization:'',
    
  };
  
  
  
  constructor(private usersClient:UsersClient,
    private addressesClient: AddressesClient,
    private localService: LocalService
    ){}
  
  
  async ngOnInit() {
    
    this.user = await firstValueFrom(this.usersClient.get(this.localService.getData('username')).pipe(
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
        address:'',
        numberOfPosts: data.numberOfPosts,
        specialization: data.specialization ?? ''

      })
      )
    ));
    this.addressesClient.get(parseInt(this.localService.getData('id'))).subscribe(
      {
        next: data => this.user.address = data.fullAddress
      }
    )

  }

  
  
}

interface IUserForSummary extends IUserDto{
  address:string,

}
