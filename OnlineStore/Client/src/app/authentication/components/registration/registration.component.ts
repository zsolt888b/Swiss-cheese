import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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

  constructor(private userService : UserService,private router : Router, private toastr : ToastrService) { }

  ngOnInit(): void {
  }

  regist(){

    if(this.chackForWhiteSpacesOrNull(this.password)==false){
      this.toastr.error("Password is required!")
      return;
    }

    if(this.chackForWhiteSpacesOrNull(this.email)==false){
      this.toastr.error("Email is required!")
      return;
    }

    if(this.chackForWhiteSpacesOrNull(this.username)==false){
      this.toastr.error("Username is required!")
      return;
    }

    if(this.chackForWhiteSpacesOrNull(this.telephoneNumber)==false){
      this.toastr.error("Telephone number is required!")
      return;
    }

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

      }
    )
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
