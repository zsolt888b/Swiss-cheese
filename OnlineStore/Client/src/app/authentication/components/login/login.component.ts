import { Component, OnInit } from '@angular/core';
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

  constructor(private userService : UserService, private authenticationService : AuthenticationService) { }

  ngOnInit(): void {
  }

  login(){

    let loginModel = new LoginModel();
    loginModel.username=this.username;
    loginModel.password=this.password

    this.userService.login(loginModel).subscribe(token =>
      {
        this.authenticationService.login(token);
      },
      error =>
      {
        console.log(error);
      })
  }
}
