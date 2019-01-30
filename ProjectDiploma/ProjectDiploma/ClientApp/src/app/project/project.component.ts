import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent {
  public projects: Project[];
  public oneProject: Project;
  public itemsLength: number;
  public pageIndex: number;
  public pageSize: number;

  pageEvent: PageEvent;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.pageSize = 2;
    this.pageIndex = 0;
  }

  ngOnInit(): void {
    this.handlePage(null);
  }
  //constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  //  http.get<Project>(baseUrl + 'api/Project/GetRandomProject').subscribe(result => {
  //    this.oneProject = result;
  //  }, error => console.error(error));
  //}
  public handlePage(event?: PageEvent) {

    this.http.get<number>(this.baseUrl + 'api/Project/GetCount').subscribe(result => {
      this.itemsLength = result;
    }, error => console.log('error in project'));

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
    }, error => console.log('error in project'));

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
  link: string;
  date: Date;
  text: string;
  tags: NewsTags;
  section: NewsType;
  sectionId: number;
}

interface ProjectStage {
  Founding,
  Start,
  Prototype,
  PreProduction,
  Production
}

interface University {
  name: string;
  contactinformation: string;
}


interface Project {
  id: number;
  name: string;
  description: string;
  risks: string;
  stage: ProjectStage;
  start: Date;
  finish: Date;
  cost: number;
  startdate: Date;
  finishdate: Date;
  costcurrent: number;
  costfull: number;
  date: Date;
  isclosed: boolean;
  initializer: University;
}
