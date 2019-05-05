import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first, startWith, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

import { AuthenticationService } from '../services/authentication.service';
import { Role } from '../models/role';
import { IOrganizationData, OrganizationType } from '../models/organization';
import { delay } from 'q';
import { Observable } from 'rxjs';
import { MatSnackBar } from '@angular/material';

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

  public selectedOrganizationInfo: string = '';

  organizations: IOrganizationData[] = [];

  public selectedRole: IRole = { value: Role.Business, viewValue: "Бизнес" };

  roles: IRole[] = [
    this.selectedRole,
    { value: Role.University, viewValue: "Университет" }
  ];




  filteredOptions: Observable<IOrganizationData[]>;

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authenticationService: AuthenticationService,
    private snackBar: MatSnackBar,
    @Inject('BASE_URL') public baseUrl: string
  ) {
    // redirect to home if already logged in
    if (this.authenticationService.currentUserValue) {
      this.router.navigate(['/']);
    }

    this.http.get<IOrganizationData[]>(this.baseUrl + 'api/Organization/GetAll').subscribe(result => {
      this.organizations = result;
    }, error => console.log('error in event'));
  }


  ngOnInit() {
    this.userForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      role: ['', Validators.required],
      organizationName: ['', Validators.required],
      contactInfo: ['', Validators.required]
    });

    this.filteredOptions = this.userForm.controls["organizationName"].valueChanges
      .pipe(
        startWith<string | IOrganizationData>(''),
        map(value => typeof value === 'string' ? value : value.name),
        map(name => name ? this._filter(name) : this.organizations.slice())
      );
  }

  getSelectedInfo(organization: IOrganizationData) {
    this.selectedOrganizationInfo = organization.contactInformation;
    this.selectedRole = organization.type == OrganizationType.Company ? this.roles[0] : this.roles[1];
  }

  displayFn(organization?: IOrganizationData): string | undefined {
    return organization ? organization.name : undefined;
  }

  private _filter(name: string): IOrganizationData[] {
    const filterValue = name.toLowerCase();

    var result = this.organizations.filter(option => option.name.toLowerCase().indexOf(filterValue) === 0);

    if (result.length == 0) {
      this.selectedOrganizationInfo = '';
    }

    return result;
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
      role: this.f.role.value,
      organization: {
        name: this.f.organizationName.value.name ? this.f.organizationName.value.name : this.f.organizationName.value,
        contactInformation: this.f.contactInfo.value,
        type: this.f.role.value == Role.University ? OrganizationType.University : OrganizationType.Company
      }
    };

    this.loading = true;
    this.http.post<object>(this.baseUrl + 'api/Account/Register', value)
      .subscribe(result => {
        this.success = true;
        this.loading = false;
      }, error => {
        this.error = error;
        this.loading = false;
        console.error(error);
      });
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
}
