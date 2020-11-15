import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { uptime } from 'process';
import { Observable } from 'rxjs';
import { UserService } from '../api/app.generated';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  providers: [UserService]
})
export class HeaderComponent implements OnInit {

  constructor(private authenticationService : AuthenticationService, private router : Router) { }

  ngOnInit(): void {
  }

  isLoggedIn() : boolean{
    return this.authenticationService.isLoggedIn;
  }

  signUp(){
    this.router.navigate(["registraion"]);
  }

  signIn(){
    this.router.navigate(["log-in"]);
  }

  home(){
    this.router.navigate([""]);
  }

  users(){
    this.router.navigate(["users"]);
  }

  profile(){
    this.router.navigate(["profile"]);
  }

  upload(){
    this.router.navigate(["upload"]);
  }

  logOut(){
    this.authenticationService.logout();
    this.router.navigate([""]);
  }


}
