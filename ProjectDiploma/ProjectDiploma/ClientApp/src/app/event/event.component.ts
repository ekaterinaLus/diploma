import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html'
})
export class EventComponent {
  public event: Event[];
  public oneEvent: Event;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get(baseUrl + 'api/Event/GetRandomEvent').subscribe(result => {
      this.oneEvent = result as Event;
    }, error => console.error(error));
  }
}


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
