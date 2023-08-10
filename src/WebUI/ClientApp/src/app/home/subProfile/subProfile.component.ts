
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
    address: '',
  };
  
  
  
  constructor(private usersClient:UsersClient,
    private addressesClient: AddressesClient,
    private localService: LocalService
    ){}
  
  
  async ngOnInit() {
    
    this.user = await firstValueFrom(this.usersClient.get(this.localService.getData('username')).pipe(
      map(data => ({
        // ...data,
        profileImage: data.profileImage != null ? data.profileImage = `https://localhost:44447/api/Images/${data.profileImage}` : data.profileImage = 'https://localhost:44447/api/Images/2b388861-8663-4843-9f65-5481388e927d_Screenshot 2023-05-06 211008.png',
        summary: data.summary != null ? data.summary : '',
        firstName: data.firstName ?? '',
        lastName: data.lastName ?? '',
        id: data.id ?? 0,
        numberOfFollowers: data.numberOfFollowers ?? 0,
        numberOfFollowings: data.numberOfFollowings ?? 0,
        role: data.role ?? '',
        userName: data.userName ?? '',
        address:''


      })
      )
    ));
    this.addressesClient.get(parseInt(this.localService.getData('id'))).subscribe(Address =>
        
        this.user.address = Address.fullAddress
      


    )
  }

  
  
}

interface IUserForSummary extends IUserDto{
  address:string,

}