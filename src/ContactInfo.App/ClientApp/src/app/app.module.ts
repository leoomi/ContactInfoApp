import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthGuard } from './guards/auth.guard';
import { LoginPageModule } from './pages/login-page/login-page.module';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { ToolbarModule } from './toolbar/toolbar.module';
import { SignUpPageComponent } from './pages/sign-up-page/sign-up-page.component';
import { SignUpPageModule } from './pages/sign-up-page/sign-up-page.module';
import { ContactListPageModule } from './pages/contact-list-page/contact-list-page.module';
import { ContactListPageComponent } from './pages/contact-list-page/contact-list-page.component';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    LoginPageModule,
    SignUpPageModule,
    ToolbarModule,
    ContactListPageModule,
    RouterModule.forRoot([
      {
        path: '', 
        component: ContactListPageComponent,
        pathMatch: 'full',
        canActivate: [ AuthGuard ]
      },
      { path: 'login', component: LoginPageComponent },
      { path: 'sign-up', component: SignUpPageComponent },
    ]),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
