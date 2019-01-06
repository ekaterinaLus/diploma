//import { Component } from '@angular/core';

//import { User } from '../models/user';
//import { AuthenticationService } from '../services/authentication.service';

//@Component({
//  selector: 'app-nav-menu',
//  templateUrl: './nav-menu.component.html',
//  styleUrls: ['./nav-menu.component.css']
//})
//export class NavMenuComponent {
//  currentUser: User;

//  constructor(
//    public authenticationService: AuthenticationService
//  ) {
//    this.currentUser = this.authenticationService.currentUserValue;
//  }
//}
import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';

/** @title Sidenav with custom escape and backdrop click behavior */

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class SidenavDisableCloseExample {
  @ViewChild('sidenav') sidenav: MatSidenav;

  reason = '';

  close(reason: string) {
    this.reason = reason;
    this.sidenav.close();
  }

  shouldRun = [/(^|\.)plnkr\.co$/, /(^|\.)stackblitz\.io$/].some(h => h.test(window.location.host));
}

