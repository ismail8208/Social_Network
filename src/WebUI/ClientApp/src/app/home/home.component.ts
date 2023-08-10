import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IUserDto } from '../web-api-client';
import { Store, select } from '@ngrx/store';
import { TokenService } from '../sheard/tokenService';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:['./home.component.css']
})
export class HomeComponent implements OnInit {

  user: IUserDto;
  userRole: any;
  token: string = "";
  isError: boolean = false;

  constructor(private authorizeService: AuthorizeService, private store: Store, private tokenService: TokenService)
  {
  }

  ngOnInit(): void {
  }

  
}
