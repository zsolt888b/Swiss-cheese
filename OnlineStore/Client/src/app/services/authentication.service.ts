import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {

  private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor() { }

  get isLoggedIn(){
    return this.loggedIn.asObservable();
  }

  login(token : string){
    if(token!=null){
      localStorage.setItem('token', token);
      this.loggedIn.next(true);
    }
  }

  logout(){
    this.loggedIn.next(false);
    localStorage.removeItem('token');
  }

  getToken() : string {
    return localStorage.getItem('token');
  }
}
