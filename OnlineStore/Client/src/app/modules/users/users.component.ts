import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserModel, UserService } from 'src/app/api/app.generated';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
  providers: [UserService]
})
export class UsersComponent implements OnInit {

  constructor(private userService : UserService, private toastr : ToastrService) { }

  users : UserModel[] = [];

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(){
    this.userService.getUsers().subscribe(res =>{
      this.users = res;
    }, error =>{
      this.toastr.error("Only administrators can access this!");
    })
  }

  save(){
    this.userService.editUsers(this.users).subscribe(res =>{
      this.toastr.success("Changes saved!");
      this.getUsers();
    });
  }

}
