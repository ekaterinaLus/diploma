import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

//Realised components
import { AppComponent } from './app.component';
/*import { NavMenuComponent } from './nav-menu/nav-menu.component';*/
import { HomeComponent } from './home/home.component';
import { NewComponent } from './news/news.component';
import { EventComponent } from './event/event.component';
import { ProjectComponent } from './project/project.component';
import { AddProject } from './project/project.add';
import { UserComponent } from './user/user.component';
import { NewsPageComponent } from './newspage/newspage';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginDialog } from './login/login.dialog';

//Material design
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSliderModule } from '@angular/material/slider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatChipsModule } from '@angular/material/chips';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material';
import { CovalentFileModule } from '@covalent/core/file';
import { MatStepperModule } from '@angular/material/stepper';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatSnackBarModule } from '@angular/material';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';


//LOGIN
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './guard/auth.guard';
import { ErrorInterceptor } from './helpers/error.interceptor';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { ProfileComponent } from './profile/profile.component';
import { ProfileMoreComponent } from './profile/profilemore.component';
import { ProjectMoreComponent } from './project/project.more';
import { ProfileViewsComponent } from './profile/profileviews.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    EventComponent,
    NewComponent,
    ProjectComponent,
    UserComponent,
    NewsPageComponent,
    LoginComponent,
    LoginDialog,
    AddProject,
    ProfileComponent,
    ProfileMoreComponent,
    ProjectMoreComponent,
    ProfileViewsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatSliderModule,
    MatProgressSpinnerModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    MatCardModule,
    MatDividerModule,
    MatMenuModule,
    MatDialogModule,
    MatSelectModule,
    MatPaginatorModule,
    MatExpansionModule,
    MatChipsModule,
    MatGridListModule,
    MatDatepickerModule,
    MatNativeDateModule,
    CovalentFileModule,
    MatStepperModule,
    MatAutocompleteModule,
    MatBadgeModule,
    MatSnackBarModule,
    MatTableModule,
    MatTabsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'event', component: EventComponent },
      { path: 'news', component: NewComponent },
      { path: 'project', component: ProjectComponent, canActivate: [AuthGuard] },
      { path: 'user', component: UserComponent },
      { path: 'login', component: LoginComponent },
      { path: 'newspage', component: NewsPageComponent },
      { path: 'newproject', component: AddProject },
      { path: 'profile', component: ProfileComponent },
      { path: 'more', component: ProfileMoreComponent },
      { path: 'views', component: ProfileViewsComponent },
      { path: 'projectmore/:projectId', component: ProjectMoreComponent, canActivate: [AuthGuard] },
    ])
  ],
  entryComponents: [NavMenuComponent, LoginDialog],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: MAT_DATE_LOCALE, useValue: 'ru-RU' }],
  bootstrap: [AppComponent]
})
export class AppModule { }

