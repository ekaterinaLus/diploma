import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Response } from '../models/response';
import { Tag } from '../models/tag';

@Injectable({ providedIn: 'root' })
export class TagService {

  private readonly endpoint: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.endpoint = `${this.baseUrl}api/Tag`;
  }

  loadTags(count: number, filter: string): Observable<Response<Tag[]>> {
    const loadEndpoint = `${this.endpoint}/Get`;

    var args = new HttpParams()
      .append("count", count.toString())
      .append("tagName", filter);

    return this.http.get<Response<Tag[]>>(loadEndpoint, { params: args });
  }
}
