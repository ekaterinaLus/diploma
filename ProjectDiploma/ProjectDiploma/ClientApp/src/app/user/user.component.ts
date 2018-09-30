import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-user',
  templateUrl: './user.component.html'
})
export class UserComponent {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  onSubmit({ value, valid }: { value: UserRegistration, valid: boolean }) {
      
    this.http.post<string>(this.baseUrl + 'api/Account/Register', value)
      .subscribe(result => { console.log(result) }, error => console.error(error));
  }
}

export interface UserRegistration {
  email: string;
  password: string;
}
