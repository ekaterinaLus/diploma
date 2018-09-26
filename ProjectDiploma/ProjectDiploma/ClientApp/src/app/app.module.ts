import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { NewComponent } from './news/news.component';
import { EventComponent } from './event/event.component';
import { ProjectComponent } from './project/project.component';
import { UserComponent } from './user/user.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    EventComponent,
    NewComponent,
    ProjectComponent,
    UserComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'event', component: EventComponent },
      { path: 'news', component: NewComponent },
      { path: 'project', component: ProjectComponent },
      { path: 'user', component: UserComponent}
    ])
  ],

  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
