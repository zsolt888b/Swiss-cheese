import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { uptime } from 'process';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private authenticationService : AuthenticationService, private router : Router) { }

  ngOnInit(): void {
  }

  isLoggedIn() : boolean{
    return this.authenticationService.isLoggedIn;
  }

  SignUp(){
    this.router.navigate(["registraion"]);
  }

  SignIn(){
    this.router.navigate(["log-in"]);
  }

  Home(){
    this.router.navigate([""]);
  }

  LogOut(){
    this.authenticationService.logout();
  }
}
