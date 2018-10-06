import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  onSubmit({ value, valid }: { value: UserLogin, valid: boolean }) {
    value.rememberme = true;
    value.returnurl = '/event';
    
    this.http.post<string>(this.baseUrl + 'api/Account/Login', value)
      .subscribe(result => { console.log(result) }, error => console.error(error));

  }
}

export interface UserLogin {
  email: string;
  password: string;
  rememberme: boolean;
  returnurl: string;
}
