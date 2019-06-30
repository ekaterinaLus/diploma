import { Component, Inject } from "@angular/core";
import { User } from '../models/user';
import { AuthenticationService } from '../services/authentication.service';
import { HttpClient } from "@angular/common/http";
import { Router } from '@angular/router';
import { Response } from '../models/response';

@Component({
  selector: 'app-profileviews',
  templateUrl: './profileviews.component.html',
  styleUrls: ['./profileviews.component.css'],

})
export class ProfileViewsComponent  {
  public currentUser: User;
  public projects: ProjectViews[];
  displayedColumns: string[] = ['projectName', 'organizationName', 'contactInformation', 'viewDate'];
  
  constructor(@Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private router: Router) {


    this.http.get<Response<ProjectViews[]>>(this.baseUrl + 'api/Project/GetProjectViews').subscribe(result => {
      this.projects = result.itemResult;
    }, error => console.log('error in event'));
  }
}

interface ProjectViews {
  projectName: string;
  organizationName: string;
  contactInformation: string;
  viewDate: Date;
}
