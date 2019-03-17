import { Component, Inject, OnInit, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FileService } from '../services/file.service';
import { Tag } from '../models/tag';
import { Observable } from 'rxjs';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatAutocomplete } from '@angular/material';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'add-project',
  templateUrl: './project.add.html',
  styleUrls: ['./project.component.css']
})
export class AddProject implements OnInit {
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;
  forthFormGroup: FormGroup;

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  tags: Tag[];
  filteredTags: Observable<Tag[]>;

  public fileToUpload: File = null;

  loading = false;
  error = '';
  success = false;
  submitted = false;

  @ViewChild('tagInput') tagInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authenticationService: AuthenticationService,
    private fileService: FileService,
    @Inject('BASE_URL') public baseUrl: string
  ) {
    this.filteredTags = this.forthFormGroup.controls["tagsCtrl"].valueChanges.pipe(
        startWith(null),
        map((tag: string | null) => tag ? this._filter(tag) : this.allFruits.slice()));
  }

  ngOnInit(): void {
    this.firstFormGroup = this.formBuilder.group({
      name: ['', Validators.required],
      start: [''],
      finish: [''],
      costCurrent: ['', [Validators.required, Validators.pattern(/^[0-9]+([.,][0-9]{2})*$/)]],
      costFull: ['', [Validators.required, Validators.pattern(/^[0-9]+([.,][0-9]{2})*$/)]]
    });

    this.secondFormGroup = this.formBuilder.group({
      description: ['', Validators.required]
    });

    this.thirdFormGroup = this.formBuilder.group({
      risks: ['']
    });

    this.forthFormGroup = this.formBuilder.group({
      
    });
  }

  //get f() { return this.projectForm.controls; }

  handleFileInput(file: File) {
    this.fileToUpload = file;
  }

  Send() {
    console.log(123);
    this.fileService.postFile(this.fileToUpload).subscribe(data => {
      console.log(data);
    }, error => {
      console.log(error);
    });
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    //if (this.projectForm.invalid) {
    //  return;
    //}
    var value = {};
    //var value = {
    //  name: this.f.name.value,
    //  startDate: this.f.start.value,
    //  finishDate: this.f.finish.value,
    //  cost: this.f.cost.value,
    //};

    //this.loading = true;
    
    this.http.post<string>(this.baseUrl + 'api/Project/Add', value)
      .subscribe(result => {
        this.success = true;
        this.loading = false;
      }, error => {
        this.error = error;
        this.loading = false;
        console.error(error);
      });
    
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.allFruits.filter(fruit => fruit.toLowerCase().indexOf(filterValue) === 0);
  }
}

