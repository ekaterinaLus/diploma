import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html'
})
export class ProjectComponent {
  public projects: Project[];
  public oneProject: Project;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Project>(baseUrl + 'api/Project/GetRandomProject').subscribe(result => {
      this.oneProject = result;
    }, error => console.error(error));
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

interface Project {
  id: number;
  name: string;
  start: Date;
  finish: Date;
  cost: number;
}
