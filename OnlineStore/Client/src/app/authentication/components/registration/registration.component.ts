import { Component, OnInit } from '@angular/core';
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

  constructor(private userService : UserService) { }

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

      },
      error => 
      {
        console.log(error);
      }
    )
  }

}
