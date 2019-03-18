import { Component, Inject, OnInit, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatChipInputEvent, MatAutocomplete } from '@angular/material';
import { Router } from '@angular/router';
import { FileService } from '../services/file.service';
import { Tag } from '../models/tag';
import { Observable } from 'rxjs';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { map, startWith } from 'rxjs/operators';
import { TagService } from '../services/tag.loading.service';

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
  tags: string[] = [];
  filteredTags: string[];

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
    private tagService: TagService,
    private authenticationService: AuthenticationService,
    private fileService: FileService,
    @Inject('BASE_URL') public baseUrl: string
  ) {
    

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
      tagsCtrl: ['']
    });

    this.forthFormGroup.controls["tagsCtrl"].valueChanges
      .pipe(startWith(null))
      .subscribe(
        (tag: string | null) => {
          this.tagService.loadTags(5, tag).subscribe(result => {
            if (!result.hasErrors) {
              this.filteredTags = result.itemResult.map(x => x.name);
            }
          }, errorResult => console.error(errorResult))
        }, errorValue => console.error(errorValue));
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
}

