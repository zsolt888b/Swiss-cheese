import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserModel, UserService } from 'src/app/api/app.generated';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
  providers: [UserService]
})
export class UsersComponent implements OnInit {

  constructor(private userService : UserService, private toastr : ToastrService,private router : Router) { }

  users : UserModel[] = [];

  ngOnInit(): void {
    this.userService.getRole().subscribe(res =>{
      if(res==false){
        this.toastr.error("Only administrators can access this!");
        this.router.navigate([""]);
      }
    })
    this.getUsers();
  }

  getUsers(){
    this.userService.getUsers().subscribe(res =>{
      this.users = res;
    }, error =>{
    })
  }

  save(){
    this.userService.editUsers(this.users).subscribe(res =>{
      this.toastr.success("Changes saved!");
      this.getUsers();
    });
  }

}
