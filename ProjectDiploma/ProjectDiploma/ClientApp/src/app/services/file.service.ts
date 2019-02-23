import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Response } from '../models/response';

@Injectable({ providedIn: 'root' })
export class FileService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  postFile(fileToUpload: File): Observable<Response<string>> {
    const endpoint = `${this.baseUrl}api/File/UploadFile`;
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    let options = new HttpHeaders({
      'enctype': 'multipart/form-data',
      'Accept': 'application/json'
    });

    return this.http
      .post<Response<string>>(endpoint, formData, { headers: options } );
  }

}
