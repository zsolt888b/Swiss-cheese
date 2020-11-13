import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './authentication/token.interceptor';
import { AuthenticationService } from './services/authentication.service';
import { RegistrationComponent } from './authentication/components/registration/registration.component';
import { FormsModule } from '@angular/forms';
import { LoginComponent } from './authentication/components/login/login.component';
import { HeaderComponent } from './header/header.component';
import { UploadComponent } from './modules/upload/upload.component';
import { HomeComponent } from './modules/home/home.component';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';
import { UsersComponent } from './modules/users/users.component';
import { FileDetailsComponent } from './modules/file-details/file-details.component';


@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    LoginComponent,
    HeaderComponent,
    UploadComponent,
    HomeComponent,
    UsersComponent,
    FileDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [
    AuthenticationService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
