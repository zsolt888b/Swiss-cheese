import { Component, OnInit } from '@angular/core';
import { FileModel, FileService, UserModel, UserService } from 'src/app/api/app.generated';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  providers: [ UserService, FileService]
})
export class ProfileComponent implements OnInit {

  constructor(private userService : UserService, private fileService : FileService) { }

  files : FileModel[];
  user : UserModel;

  ngOnInit(): void {
    this.getData();
  }

  getData(){
    this.userService.getUser().subscribe(res =>{
      this.user = res;
      this.fileService.getMyFiles().subscribe(files =>{
        this.files = files;
      })
    })
  }

}
