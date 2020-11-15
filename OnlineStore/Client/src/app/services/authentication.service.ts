import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { runInThisContext } from 'vm';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {


  private loggedIn: boolean = false;

  constructor() {
    if(this.getToken()!=null){
      this.loggedIn = true;
    }else{
      this.loggedIn = false;
    }
   }

  get isLoggedIn(){
    return this.loggedIn;
  }

  login(token : string){
    if(token!=null){
      localStorage.setItem('token', token);
      this.loggedIn = true;
    }
  }

  logout(){
    this.loggedIn = false;
    localStorage.removeItem('token');
  }

  getToken() : string {
    return localStorage.getItem('token');
  }
}
