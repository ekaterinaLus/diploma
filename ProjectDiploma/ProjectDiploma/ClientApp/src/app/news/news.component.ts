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
  public oneNews: News;
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

  //public getNews(id1: number) {
  //  var args = new HttpParams()
  //    .append("id", this.oneNews.id.toString());
  //  this.http.get(this.baseUrl + 'api/News/Get/' + id1
  //  ).subscribe(result =>
  //  {
  //    if (this.oneNews.id = id1) {
  //      result = this.oneNews;
  //    }
  //  }, err => console.log('error in news'));
  //}


  public getNews(id: number) {
    this.http.get<News>(this.baseUrl + 'api/News/Get/' + id
    ).subscribe(result => {
      this.oneNews = result
      console.log(result);
    }, err => console.log('error in news'));
  }

  //public getNews() {
  //  this.http.get(this.baseUrl + 'api/News/Get').subscribe(result => {
  //  }, err => console.log('error in news'));
  //}

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
