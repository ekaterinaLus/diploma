import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PageEvent } from '@angular/material';


@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {
  
  public events: Event[];
  public event: Event;
  public itemsLength: number;
  public pageIndex: number;
  public pageSize: number;

  pageEvent: PageEvent;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.pageSize = 5;
    this.pageIndex = 0;
  }

  public FindEvent(): void {
    this.http.post(this.baseUrl + 'api/Event/FindEvent', this.event)
      .subscribe(result => { }, error => console.error(error))
  }

  ngOnInit(): void {
    this.handlePage(null);
  }

  public handlePage(event?: PageEvent) {

    this.http.get<number>(this.baseUrl + 'api/Event/GetCount').subscribe(result => {
      this.itemsLength = result;
    }, error => console.log('error in event'));

    if (event != null) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }

    var args = new HttpParams()
      .append("pageIndex", this.pageIndex.toString())
      .append("pageSize", this.pageSize.toString());

    this.http.get<Event[]>(this.baseUrl + 'api/Event/GetPage', {
      params: args
    }).subscribe(result => {
      this.events = result;
    }, error => console.log('error in event'));

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
  address: string;
  cost?: number;
  tags: Tag[];
}

interface Tag {
  id: number;
  name: string;
}
interface News {
  id: number;
  header: string;
  annotation: string;
  date: Date;
  text: string;
  tags: NewsTags;
  section: NewsType;
  sectionId: number;
}
