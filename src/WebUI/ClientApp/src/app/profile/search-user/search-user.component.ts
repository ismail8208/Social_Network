import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs';
import { IUserDto, UsersClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-search-user',
  templateUrl: './search-user.component.html',
  styleUrls: ['./search-user.component.css']
})
export class SearchUserComponent implements OnInit {

  myvar: string;
  users: IUserDto[] = [];
  constructor(private usersClient: UsersClient) { }

  ngOnInit(): void {
  
  }

  public set mylistFilter(user : string) {
    this.usersClient.search(user).pipe(
      map(users => users.items.map(u => ({
        ...u,
        profileImage: u.profileImage != null ? `https://localhost:44447/api/Images/${u.profileImage}` : 'https://localhost:44447/api/Images/f08c0eb9-cdde-471c-af59-a83005ea784f_Screenshot_٢٠٢٠-٠٩-٢٠-١٦-٤٤-١١.png'

      })))).subscribe({
      next: data => {
        this.users = data; 
      }
    });
  }

  openD() {
    const modal = document.querySelector('.search-result') as HTMLElement;
    modal.style.opacity = '1';
    modal.style.pointerEvents = 'auto';
  }
  
  closeD(){
    const modal = document.querySelector('.search-result') as HTMLElement;
    modal.style.opacity = '0';
    modal.style.pointerEvents = 'none';
    this.users = [];
  }

}
