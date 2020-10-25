import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RegistrationModel, UserService } from 'src/app/api/app.generated';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
  providers: [ UserService]
})
export class RegistrationComponent implements OnInit {

  email : string;
  password : string;
  username : string;
  telephoneNumber : string;

  constructor(private userService : UserService,private router : Router) { }

  ngOnInit(): void {
  }

  regist(){

    let registrationModel = new RegistrationModel();
    registrationModel.email=this.email;
    registrationModel.password=this.password;
    registrationModel.telephoneNumber=this.telephoneNumber;
    registrationModel.username=this.username;

    this.userService.register(registrationModel).subscribe(res =>
      {
        this.router.navigate(["log-in"]);
      },
      error =>
      {
        console.log(error);
      }
    )
  }

}
