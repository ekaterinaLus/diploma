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
import { MatSnackBar } from '@angular/material';

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

  projectName: string;
  fileImage: string;
  projectDescription: string;

  @ViewChild('tagInput') tagInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private tagService: TagService,
    private authenticationService: AuthenticationService,
    private fileService: FileService,
    private snackBar: MatSnackBar,
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
      .subscribe(
        (tag: string | null) => {
          this.tagService.loadTags(5, tag).subscribe(result => {
            if (!result.hasErrors) {
              this.filteredTags = result.itemResult.map(x => x.name);
            }
          }, errorResult => console.error(errorResult))
        }, errorValue => console.error(errorValue));

    this.firstFormGroup.controls["name"].valueChanges
      .subscribe((name: string) => this.projectName = name);

    this.secondFormGroup.controls["description"].valueChanges
      .subscribe((desc: string) => this.projectDescription = desc);
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.tags.push(event.option.viewValue);
    this.tagInput.nativeElement.value = '';
    this.forthFormGroup.controls["tagsCtrl"].setValue(null);
  }

  //get f() { return this.projectForm.controls; }

  handleFileInput(file: File) {
    if (file) {
      this.fileToUpload = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.fileImage = reader.result.toString();
      };
      reader.readAsDataURL(file);
    }

  }

  Send() {
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

    //stop here if form is invalid
    //if (this.projectForm.invalid) {
    //  return;
    //}

    if (this.fileToUpload != null) {
      this.fileService.postFile(this.fileToUpload).subscribe(data => {

        var loadedFileName: string = null;
        if (!data.hasErrors) {
          loadedFileName = data.itemResult;
        }

        this.CreateProject(loadedFileName);
      }, error => {
        this.CreateProject(null);
      });
    } else {
      this.CreateProject(null);
    }

    //this.CreateProject(null);

  }

  remove(tagItem: string): void {
    const index = this.tags.indexOf(tagItem);

    if (index >= 0) {
      this.tags.splice(index, 1);
    }
  }

  private CreateProject(loadedFileName: string) {
    var tagObjects = this.tags.map<Tag>(x => { return { id: 0, name: x }; });
    var value = {
      name: this.firstFormGroup.controls.name.value,
      description: this.secondFormGroup.controls.description.value,
      risks: this.thirdFormGroup.controls.risks.value,
      startDate: this.firstFormGroup.controls.start.value,
      finishDate: this.firstFormGroup.controls.finish.value,
      costCurrent: this.firstFormGroup.controls.costCurrent.value,
      costFull: this.firstFormGroup.controls.costFull.value,
      fileName: loadedFileName,
      tags: tagObjects
    };
    this.loading = true;
    this.http.post<string>(this.baseUrl + 'api/Project/Add', value)
      .subscribe(result => {
        this.success = true;
        this.loading = false;

        this.router.navigate(['/project'])
      }, error => {
        this.error = error;
        this.loading = false;
        console.error(error);
      });
  }

}

