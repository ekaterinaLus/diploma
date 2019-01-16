import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PageEvent } from '@angular/material';


@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})


export class NewComponent implements OnInit{
  public news: News[];
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

  public handlePage(event?: PageEvent) {

    this.http.get<number>(this.baseUrl + 'api/News/GetCount').subscribe(result => {
      this.itemsLength = result;
    }, error => console.log('error in news'));

    if (event != null) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }

    var args = new HttpParams()
      .append("pageIndex", this.pageIndex.toString())
      .append("pageSize", this.pageSize.toString());

    this.http.get<News[]>(this.baseUrl + 'api/News/GetPage', {
      params: args
    }).subscribe(result => {
      this.news = result;
    }, error => console.log('error in news'));

  }
  //constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  //  http.get<News>(baseUrl + 'api/News/GetRandomNews').subscribe(result => {
  //    this.oneNews = result;
  //  }, error => console.error(error));
  //}
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
