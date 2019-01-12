import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

import { AuthenticationService } from '../services/authentication.service';
import { Role } from '../models/role';

export interface IRole {
  value: Role,
  viewValue: string
}

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  userForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  success = false;
  roles: IRole[] = [
    { value: Role.Business, viewValue: "Бизнес" },
    { value: Role.University, viewValue: "Университет" }
  ];

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authenticationService: AuthenticationService,
    @Inject('BASE_URL') public baseUrl: string
  ) {
    // redirect to home if already logged in
    if (this.authenticationService.currentUserValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.userForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      role: ['', Validators.required]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.userForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.userForm.invalid) {
      return;
    }

    var value = {
      email: this.f.username.value,
      password: this.f.password.value,
      role: this.f.role.value
    };

    this.loading = true;
    this.http.post<string>(this.baseUrl + 'api/Account/Register', value)
      .subscribe(result => {
        this.success = true;
        this.loading = false;
      }, error => {
        this.error = error;
        this.loading = false;
        console.error(error);
      });
    //delay(2000);
    //this.success = true;
    //this.loading = false;
  }
}
