import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first, startWith, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

import { AuthenticationService } from '../services/authentication.service';
import { Role } from '../models/role';
import { IOrganizationData, OrganizationType } from '../models/organization';
import { delay } from 'q';
import { Observable } from 'rxjs';
import { MatSnackBar, MatAutocomplete, MatChipInputEvent, MatAutocompleteSelectedEvent } from '@angular/material';
import { Message, MessageType, Response } from "../models/response";
import { TagService } from '../services/tag.loading.service';
import { Tag } from '../models/tag';

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
  tags: string[] = [];
  filteredTags: string[];

  public selectedOrganizationInfo: string = '';

  organizations: IOrganizationData[] = [];

  public selectedRole: IRole = { value: Role.Business, viewValue: "Бизнес" };

  roles: IRole[] = [
    this.selectedRole,
    { value: Role.University, viewValue: "Университет" }
  ];

  filteredOptions: Observable<IOrganizationData[]>;

  @ViewChild('tagInput') tagInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authenticationService: AuthenticationService,
    private snackBar: MatSnackBar,
    private tagService: TagService,
    @Inject('BASE_URL') public baseUrl: string
  ) {
    // redirect to home if already logged in
    if (this.authenticationService.currentUserValue) {
      this.router.navigate(['/']);
    }

    this.http.get<IOrganizationData[]>(this.baseUrl + 'api/Organization/GetAll').subscribe(result => {
      this.organizations = result;
    }, error => console.log('error in organization'));
  }


  ngOnInit() {
    this.userForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      role: ['', Validators.required],
      organizationName: ['', Validators.required],
      contactInfo: ['', Validators.required],
      tagsCtrl: ['']
    });

    this.filteredOptions = this.userForm.controls["organizationName"].valueChanges
      .pipe(
        startWith<string | IOrganizationData>(''),
        map(value => typeof value === 'string' ? value : value.name),
        map(name => name ? this._filter(name) : this.organizations.slice())
    );

    this.userForm.controls["tagsCtrl"].valueChanges
      .subscribe(
        (tag: string | null) => {
          this.tagService.loadTags(5, tag).subscribe(result => {
            if (!result.hasErrors) {
              this.filteredTags = result.itemResult.map(x => x.name);
            }
          }, errorResult => console.error(errorResult))
        }, errorValue => console.error(errorValue));
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

    var tagObjects = this.tags.map<Tag>(x => { return { id: 0, name: x }; });

    var value = {
      email: this.f.username.value,
      password: this.f.password.value,
      role: this.f.role.value,
      organization: {
        name: this.f.organizationName.value.name ? this.f.organizationName.value.name : this.f.organizationName.value,
        contactInformation: this.f.contactInfo.value,
        type: this.f.role.value == Role.University ? OrganizationType.University : OrganizationType.Company
      },
      tags: tagObjects
    };

    this.loading = true;
    this.http.post<Response<any>>(this.baseUrl + 'api/Account/Register', value)
      .subscribe(result => {
        this.success = result.hasErrors;
        this.loading = false;

        if (result.hasErrors) {
          var errors = result.messages.filter(x => x.messageType != MessageType.ERROR);
          this.openSnackBar(errors[0].text, "Ошибка");
        }
      }, error => {
        this.error = error;
        this.loading = false;
        console.error(error);
      });
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 4500,
    });
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.tags.push(event.option.viewValue);
    this.tagInput.nativeElement.value = '';
    this.userForm.controls["tagsCtrl"].setValue(null);
  }

  add(event: MatChipInputEvent): void {
    // Add fruit only when MatAutocomplete is not open
    // To make sure this does not conflict with OptionSelected Event
    if (!this.matAutocomplete.isOpen) {
      const input = event.input;
      const value = event.value;

      // Add our fruit
      if ((value || '').trim()) {
        this.tags.push(value.trim());
      }

      // Reset the input value
      if (input) {
        input.value = '';
      }

      this.filteredTags = null;
    }
  }
}
