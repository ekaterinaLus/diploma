import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Role } from '../models/role';
import { User } from '../models/user';
import { AuthenticationService } from '../services/authentication.service';
import { FileService } from '../services/file.service';
import { Response } from '../models/response';

@Component({
  selector: 'app-project-more',
  templateUrl: './project.more.html',
  styleUrls: ['./project.more.css']
})
export class ProjectMoreComponent implements OnInit {
  public url: string;
  public project: Project;
  
  currentUser: User;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    public authenticationService: AuthenticationService,
    private fileService: FileService,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {

    var id = route.snapshot.paramMap.get("projectId");

    this.http.get <Response<Project>>(this.baseUrl + 'api/Project/Get/' + id.toString()).subscribe(result => {
      this.project = result.itemResult;
    }, error => console.log(error));

    this.url = this.fileService.loadFileUrl();

    this.authenticationService.currentUser.subscribe(x => {
      if (x != null) {
        this.currentUser = x;
      }
      else {
        this.currentUser = null;
      }
    });
  }

  public onLike(id: number) {

    var args = new HttpParams()
      .append("projectId", id.toString())
      .append("interest", "1");

    this.http.post<any>(this.baseUrl + 'api/NeuralNetwork/Train', null, { params: args }).subscribe(() => {
    }, error => console.log(error));
  }

  public onDislike(id: number) {

    var args = new HttpParams()
      .append("projectId", id.toString())
      .append("interest", "0");

    this.http.post<any>(this.baseUrl + 'api/NeuralNetwork/Train', null, { params: args }).subscribe(() => {
    }, error => console.log(error));
  }

  public subscribe(id: number) {
    this.http.post<any>(this.baseUrl + 'api/Project/Subscribe/' + id.toString(), null).subscribe(() => {
    }, error => console.log(error));
  }


  ngOnInit(): void {
    
  }

  get canAdd() {
    return this.currentUser && (this.currentUser.role == Role.Admin || this.currentUser.role == Role.University);
  }

  get canRate() {
    return this.currentUser && (this.currentUser.role == Role.Admin || this.currentUser.role == Role.Business);
  }

  public back() {
    this.router.navigate(['/project']);
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
