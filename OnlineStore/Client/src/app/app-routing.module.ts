import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './authentication/auth.guard';
import { LoginComponent } from './authentication/components/login/login.component';
import { RegistrationComponent } from './authentication/components/registration/registration.component';
import { NotauthGuard } from './authentication/notauth.guard';
import { FileDetailsComponent } from './modules/file-details/file-details.component';
import { HomeComponent } from './modules/home/home.component';
import { ProfileComponent } from './modules/profile/profile.component';
import { UploadComponent } from './modules/upload/upload.component';
import { UsersComponent } from './modules/users/users.component';

const routes: Routes = [
  { path: '',                 component: HomeComponent, pathMatch: 'full' },
  { path : "registraion",     component: RegistrationComponent, canActivate: [NotauthGuard] },
  { path : "log-in",          component: LoginComponent, canActivate: [NotauthGuard] },
  { path : "upload",          component: UploadComponent , canActivate: [AuthGuard]},
  { path : "users",           component: UsersComponent , canActivate: [AuthGuard]},
  { path : "profile",         component: ProfileComponent , canActivate: [AuthGuard]},
  { path: 'files/:id' ,       component: FileDetailsComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
