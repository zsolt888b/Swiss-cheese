import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FileService, LoginModel, UserService } from 'src/app/api/app.generated';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [
    UserService
  ]
})
export class LoginComponent implements OnInit {

  username : string;
  password : string;

  constructor(private userService : UserService,
    private authenticationService : AuthenticationService,
    private router : Router,  private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  login(){

    if(this.chackForWhiteSpacesOrNull(this.username)==false){
      this.toastr.error("Username is required!")
      return;
    }

    if(this.chackForWhiteSpacesOrNull(this.password)==false){
      this.toastr.error("Password is required!")
      return;
    }

    let loginModel = new LoginModel();
    loginModel.username=this.username;
    loginModel.password=this.password

    this.userService.login(loginModel).subscribe(token =>
      {
        this.authenticationService.login(token);
        this.router.navigate([""]);
      },
      error =>
      {

      })
  }

  chackForWhiteSpacesOrNull(string : String) : Boolean{
    if(string==null){
      return false;
    }
    if (!string.replace(/\s/g, '').length) {
      return false;
    }
    return true;
  }
}
