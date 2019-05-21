import { Component, Inject } from "@angular/core";
import { User } from '../models/user';
import { AuthenticationService } from '../services/authentication.service';
import { HttpClient } from "@angular/common/http";
import { Router } from '@angular/router';

@Component({
  selector: 'app-profilemore',
  templateUrl: './profilemore.component.html',
  styleUrls: ['./profilemore.component.css'],

})
export class ProfileMoreComponent  {
  public currentUser: User;
  public projects: Project[];
  displayedColumns: string[] = ['name', 'contactInformation' ];
  
  constructor(@Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private router: Router) {


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
