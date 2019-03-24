import { Component } from "@angular/core";
import { Router, NavigationEnd } from '@angular/router';
import { User } from '../models/user';
import { AuthenticationService } from '../services/authentication.service';
import { MatDialog } from '@angular/material';
import { filter, first } from 'rxjs/operators';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],

})
export class ProfileComponent  {
  public currentUser: User;
  public showLoginButton: boolean;

  constructor(public authenticationService: AuthenticationService, private router: Router, private dialog: MatDialog) {
    this.authenticationService.currentUser.subscribe(x => {
      if (x != null) {
        this.currentUser = x;
      }
      else {
        this.currentUser = null;
      }
    });

    this.router.events.pipe(filter((event: any) => event instanceof NavigationEnd))
      .subscribe(event => { this.showLoginButton = !event.url.startsWith("/login"); });
  }
}
