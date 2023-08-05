import { Component } from '@angular/core';
import { LocalService } from './sheard/localService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  constructor(private localStore: LocalService) {}
  
}
