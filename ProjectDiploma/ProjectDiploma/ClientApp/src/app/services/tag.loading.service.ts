import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Response } from '../models/response';
import { Tag } from '../models/tag';

@Injectable({ providedIn: 'root' })
export class TagService {

  private readonly endpoint: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.endpoint = `${this.baseUrl}api/Tag`;
  }

  loadTags(count: number, filter: string): Observable<Response<string>> {
    const loadEndpoint = `${this.endpoint}/Get`;

    return this.http
      .get<Response<Tag>>(loadEndpoint, { count, filter });
  }

  loadFileUrl(): string {
    return `${this.endpoint}/LoadFile/`;
  }

}
