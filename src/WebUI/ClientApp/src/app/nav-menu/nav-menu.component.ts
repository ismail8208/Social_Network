import { Component, HostListener, OnInit } from '@angular/core';
import { Observable, map} from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {

  public isAuthenticated?: Observable<boolean>;
  public userName?: Observable<string | null | undefined>;

  constructor(private authorizeService: AuthorizeService) {
   }
  async ngOnInit() {    
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
  }


  isSticky: boolean = false;

  @HostListener('window:scroll', ['$event'])
  handleScroll() {
    this.isSticky = window.pageYOffset >= 80; // Change 100 to the desired scroll position
  }
  

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
