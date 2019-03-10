import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FileService } from '../services/file.service';

@Component({
  selector: 'add-project',
  templateUrl: './project.add.html',
  styleUrls: ['./project.component.css']
})
export class AddProject implements OnInit {
  projectForm: FormGroup;

  public fileToUpload: File = null;

  loading = false;
  error = '';
  success = false;
  submitted = false;
  
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authenticationService: AuthenticationService,
    private fileService: FileService,
    @Inject('BASE_URL') public baseUrl: string
  ) {
        
  }

  ngOnInit(): void {
    this.projectForm = this.formBuilder.group({
      name: ['', Validators.required],
      start: [''],
      finish: [''],
      cost: ['']
    });
  }

  get f() { return this.projectForm.controls; }

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
    if (this.projectForm.invalid) {
      return;
    }

    var value = {
      name: this.f.name.value,
      startDate: this.f.start.value,
      finishDate: this.f.finish.value,
      cost: this.f.cost.value,
    };

    this.loading = true;

    

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

