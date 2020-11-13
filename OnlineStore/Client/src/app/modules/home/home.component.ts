import { Component, OnInit } from '@angular/core';
import { CommentModel, FileModel, FileService } from 'src/app/api/app.generated';
import { saveAs } from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [ FileService]
})
export class HomeComponent implements OnInit {

  constructor(private fileService : FileService, private toastr: ToastrService, private authenticationService : AuthenticationService, private router : Router) { }

  files : FileModel[];
  fileName : string;

  ngOnInit(): void {
    this.getFiles();
  }

  getFiles(){
    this.fileService.getFiles(this.fileName).subscribe(res =>{
      this.files=res;
    })
  }

  download(id : number){
    if(!this.authenticationService.isLoggedIn){
      this.toastr.error("You have to be logged-in to download!")
      return;
    }

    this.fileService.download(id).subscribe(res =>{
      saveAs(res.data, res.fileName);
    }, error =>{
      this.toastr.error("Not enough money!");
    })
  }

  navigateToDetails(id : number){
    this.router.navigate(['files', id]);
  }

  deleteFile(id : number){
    this.fileService.deleFile(id).subscribe(res =>{
      this.toastr.success("File deleted!")
      this.getFiles();
    }, error =>{
      this.toastr.error("Only administrators can access this!");
    })
  }
}
