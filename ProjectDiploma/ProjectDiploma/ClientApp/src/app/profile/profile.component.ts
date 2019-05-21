import { Component, Inject } from "@angular/core";
import { User } from '../models/user';
import { AuthenticationService } from '../services/authentication.service';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],

})
export class ProfileComponent  {
  public currentUser: User;
  public projects: Project[];
  
  constructor(@Inject('BASE_URL') private baseUrl: string,
              public authenticationService: AuthenticationService,
              private http: HttpClient) {
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
    }, error => console.log('error in event'));
  }
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
