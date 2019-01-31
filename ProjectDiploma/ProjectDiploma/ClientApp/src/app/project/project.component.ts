import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PageEvent } from '@angular/material';
import { User } from '../models/user';
import { AuthenticationService } from '../services/authentication.service';
import { Role } from '../models/role';


@Component({
  selector: 'app-project',
  templateUrl: './project.component.html'
})
export class ProjectComponent implements OnInit {
  public projects: Project[];
  public itemsLength: number;
  public pageIndex: number;
  public pageSize: number;

  pageEvent: PageEvent;

  currentUser: User;

  constructor(public authenticationService: AuthenticationService, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.pageSize = 1;
    this.pageIndex = 0;

    this.authenticationService.currentUser.subscribe(x => {
      if (x != null) {
        this.currentUser = x;
      }
      else {
        this.currentUser = null;
      }
    });
  }

  ngOnInit(): void {
    this.handlePage(null);
  }

  get canAdd() {
    return this.currentUser && (this.currentUser.role == Role.Admin || this.currentUser.role == Role.University);
  }

  public handlePage(event?: PageEvent) {

    this.http.get<number>(this.baseUrl + 'api/Project/GetCount').subscribe(result => {
      this.itemsLength = result;
    }, error => console.log('error in event'));

    if (event != null) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }

    var args = new HttpParams()
      .append("pageIndex", this.pageIndex.toString())
      .append("pageSize", this.pageSize.toString());

    this.http.get<Project[]>(this.baseUrl + 'api/Project/GetPage', {
      params: args
    }).subscribe(result => {
      this.projects = result;
      console.log(1);
      console.log(this.projects);
    }, error => console.log('error in event'));

  }
}

//прописать ньюс таг

interface NewsTags {
  newsId: number;
  news: News;
  tagsId: number;
  tags: Tag;
}

interface NewsType {
  id: number;
  name: string;
}

interface Event {
  id: number;
  date: Date;
  title: string;
  description: string;
  adress: string;
  cost: number;
  tags: Tag;
}

interface EventsTags {
  eventId: number;
  events: Event;
  tagsId: number;
  tags: Tag;
}

interface Tag {
  id: number;
  name: string;
  news: NewsTags;
  events: EventsTags;
}
interface News {
  id: number;
  header: string;
  annotation?: string;
  date: Date;
  text: string;
  tags: NewsTags;
  section: NewsType;
  sectionId: number;
}

interface Company {
  id: number;
  name: string;
  contactInformation: string;
}

interface Project {
  id: number;
  name: string;
  start: Date;
  finish: Date;
  cost: number;
  initializer: Company;
}
