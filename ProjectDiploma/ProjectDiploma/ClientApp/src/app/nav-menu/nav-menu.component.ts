import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Router, NavigationEnd } from '@angular/router';
import { MatDialog } from '@angular/material';

import { AuthenticationService } from '../services/authentication.service';
import { User } from '../models/user';
import { filter, first } from 'rxjs/operators';

import { LoginDialog } from '../login/login.dialog';

/** @title Sidenav with custom escape and backdrop click behavior */

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  @ViewChild('sidenav') sidenav: MatSidenav;
  public currentUser: string;
  public showLoginButton: boolean;

  constructor(public authenticationService: AuthenticationService, private router: Router, private dialog: MatDialog) {
    this.authenticationService.currentUser.subscribe(x =>
    {
      if (x != null) {
        this.currentUser = x.email
      }
      else {
        this.currentUser = null;
      }
    });

    this.router.events.pipe(filter((event: any) => event instanceof NavigationEnd))
      .subscribe(event => { this.showLoginButton = !event.url.startsWith("/login"); });
  }

  public openLoginDialog() {
    const dialogRef = this.dialog.open(LoginDialog, {
      autoFocus: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.authenticationService.login(result.username, result.password)
          .pipe(first())
          .subscribe();
      }
    });
  }

  public logout() {
    this.authenticationService.logout();
  }
  
  close(reason: string) {
    this.sidenav.close();
  }
}

