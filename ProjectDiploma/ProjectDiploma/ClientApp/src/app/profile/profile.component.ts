import { Component, Inject } from "@angular/core";
import { User } from '../models/user';
import { AuthenticationService } from '../services/authentication.service';
import { HttpClient } from "@angular/common/http";
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],

})
export class ProfileComponent  {
  public currentUser: User;
  public projects: Project[];
  public subscribesCount: number;
  
  constructor(@Inject('BASE_URL') private baseUrl: string,
              public authenticationService: AuthenticationService,
              private http: HttpClient,
              private router: Router) {
    this.authenticationService.currentUser.subscribe(x => {
      if (x != null) {
        this.currentUser = x;
      }
      else {
        this.currentUser = null;
      }
    });

    this.http.get<Project[]>(this.baseUrl + 'api/Project/GetProjectsByUser').subscribe(result => {
      this.projects = result;

      this.http.get<number>(this.baseUrl + 'api/Project/GetSubscribersCount').subscribe(result => {
        this.subscribesCount = result;
      }, error => console.log('error in count loading'));
    }, error => console.log('error in event'));
  }

  clickMore() {
    this.http.get<void>(this.baseUrl + 'api/Project/CleanHistory').subscribe(result => {
      this.router.navigate(['/more']);
    }, error => console.log('error in count loading'));    
  }

  clickHistory() {
    this.router.navigate(['/views']);    
  }

  //clickHistory
}

//прописать ньюс таг
interface Tag {
  id: number;
  name: string;
}

interface Company {
  id: number;
  name: string;
  contactInformation: string;
}

interface Project {
  id: number;
  name: string;
  startDate: Date;
  finishDate: Date;
  cost: number;
  fileName: string;
  initializer: Company;
  sponsors: Company[];
  tags: Tag[];
  description: string;
  rate: number;
}
