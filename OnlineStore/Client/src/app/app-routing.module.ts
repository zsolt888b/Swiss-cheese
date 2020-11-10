import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './authentication/components/login/login.component';
import { RegistrationComponent } from './authentication/components/registration/registration.component';
import { HomeComponent } from './modules/home/home.component';
import { UploadComponent } from './modules/upload/upload.component';
import { UsersComponent } from './modules/users/users.component';

const routes: Routes = [
  { path: '',                 component: HomeComponent, pathMatch: 'full'},
  { path : "registraion",     component: RegistrationComponent},
  { path : "log-in",          component: LoginComponent },
  { path : "upload",          component: UploadComponent },
  { path : "users",           component: UsersComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
